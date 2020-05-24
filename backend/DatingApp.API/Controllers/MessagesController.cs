using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : CustomControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public MessagesController(IDatingRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        // api/users/{userId}/messages/{id}
        [HttpGet("{id}", Name = nameof(GetMessage))]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);
            if (messageFromRepo == null)
                return NotFound();

            var userFromRepo = await _repo.GetUser(userId);
            if (!userFromRepo.MessagesSent.Any(p => p.Id == id) && !userFromRepo.MessagesReceived.Any(p => p.Id == id))
                return Unauthorized();

            var messageToReturn = _mapper.Map<MessageToReturnDto>(messageFromRepo);

            return Ok(messageToReturn);
        }

        // api/users/{userId}/messages
        [HttpGet]
        public async Task<IActionResult> GetMessageForUser(int userId, [FromQuery]MessageParams messageParams)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            messageParams.UserId = userId;

            var messagesFromRepo = await _repo.GetMessagesForUser(messageParams);

            var messagesToReturn = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize,
                messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            return Ok(messagesToReturn);
        }

        // api/users/{userId}/messages/thread/{recipientId}
        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessagesThread(int userId, int recipientId)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            var messagesThreadFromRepo = await _repo.GetMessagesThread(userId, recipientId);

            var messagesThreadToReturn = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesThreadFromRepo);

            return Ok(messagesThreadToReturn);
        }

        // api/users/{userId}/messages
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, [FromBody]MessageForCreationDto messageForCreationDTO)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            messageForCreationDTO.SenderId = userId;

            if (await _repo.GetUser(messageForCreationDTO.RecipientId) == null)
                return BadRequest("Could not find user");

            var message = _mapper.Map<Message>(messageForCreationDTO);

            _repo.Add<Message>(message);

            if (await _repo.SaveAll())
            {
                var messageToReturn = _mapper.Map<MessageToReturnDto>(await _repo.GetMessage(message.Id));
                return CreatedAtRoute(nameof(GetMessage), new { Id = message.Id }, messageToReturn);
            }

            return BadRequest("Creating the message failed on save");
        }

        // api/users/{userId}/messages/{id}/delete
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteMessage(int userId, int id)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                _repo.Delete(messageFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Error deleting the message");
        }

        // api/users/{userId}/messages/thread/{recipientId}/mark-as-read
        [HttpPost("thread/{recipientId}/mark-as-read")]
        public async Task<IActionResult> MarkMessagesOfSenderAsRead(int userId, int recipientId)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            if (userId == recipientId)
                return Unauthorized();

            var messagesFromRepo = await _repo.GetSenderMessagesThread(userId, recipientId);
            foreach (var message in messagesFromRepo)
            {
                // security check
                if (message.RecipientId != userId)
                    return Unauthorized();
                if (message.SenderId != recipientId)
                    return Unauthorized();

                // only mark as read the unread messages
                if (message.IsRead == false)
                {
                    message.IsRead = true;
                    message.DateRead = DateTimeOffset.Now;
                }
            }

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Error marking the message as read");
        }
    }
}