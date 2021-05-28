using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Core.Dtos;
using DatingApp.Api.Helpers;
using DatingApp.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    // [Authorize] // once we are using AspNet Core Identity, we dont need this line anymore
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : CustomControllerBase
    {
        private readonly IMessageService _service;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public MessagesController(IMessageService service, IUserService userService, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // api/users/{userId}/messages/{id}
        [HttpGet("{id}", Name = nameof(GetMessage))]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            var messageFromRepo = await _service.GetMessage(id);
            if (messageFromRepo == null)
                return NotFound();

            var userFromRepo = await _userService.GetUser(userId, true);
            if (!userFromRepo.MessagesSent.Any(p => p.Id == id) && !userFromRepo.MessagesReceived.Any(p => p.Id == id))
                return Unauthorized();

            var messageToReturn = _mapper.Map<MessageToReturnDto>(messageFromRepo);

            return Ok(messageToReturn);
        }

        // api/users/{userId}/messages
        [HttpGet]
        public async Task<IActionResult> GetMessageForUser(int userId, [FromQuery] MessageFilterDto filterDto)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            filterDto.UserId = userId;

            var messagesFromRepo = await _service.GetMessagesForUser(filterDto);

            var messagesToReturn = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            Response.AddPagination(
                messagesFromRepo.CurrentPage,
                messagesFromRepo.PageSize,
                messagesFromRepo.TotalCount,
                messagesFromRepo.TotalPages
            );

            return Ok(messagesToReturn);
        }

        // api/users/{userId}/messages/thread/{recipientId}
        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessagesThread(int userId, int recipientId)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            var messagesThreadFromRepo = await _service.GetMessagesThread(userId, recipientId);

            var messagesThreadToReturn = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesThreadFromRepo);

            return Ok(messagesThreadToReturn);
        }

        // api/users/{userId}/messages
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, [FromBody] MessageForCreationDto messageDto)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            messageDto.SenderId = userId;

            var recipient = await _userService.GetUser(messageDto.RecipientId, false);
            if (recipient == null)
                return BadRequest($"Could not find user {messageDto.RecipientId}.");

            var message = await _service.SaveMessage(messageDto);

            var currentMessage = await _service.GetMessage(message.Id);
            var messageToReturn = _mapper.Map<MessageToReturnDto>(currentMessage);
            return Ok(messageToReturn);
        }

        // api/users/{userId}/messages/{id}/delete
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteMessage(int userId, int id)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            await _service.DeleteMessage(id, userId);

            return NoContent();
        }

        // api/users/{userId}/messages/thread/{recipientId}/mark-as-read
        [HttpPost("thread/{recipientId}/mark-as-read")]
        public async Task<IActionResult> MarkMessagesOfSenderAsRead(int userId, int recipientId)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            await _service.MarkMessagesOfSenderAsRead(userId, recipientId);

            return NoContent();
        }
    }
}