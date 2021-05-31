using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Api.Helpers;
using DatingApp.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using DatingApp.Core.Dtos.Messages;
using DatingApp.Core.Interfaces;

namespace DatingApp.Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/messages")]
    [ApiController]
    public class MessagesController : CustomControllerBase
    {
        private readonly IMessageService _service;
        private readonly IUserService _userService;
        private readonly IClassMapper _mapper;

        public MessagesController(IMessageService service, IUserService userService, IClassMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // api/messages/{id}
        [HttpGet("{id}", Name = nameof(GetMessage))]
        public async Task<IActionResult> GetMessage(int id)
        {
            var message = await _service.GetMessage(id);
            if (message == null)
                return NotFound();

            var user = await _userService.GetUser(base.GetUserIdFromToken(), true);
            if (!user.MessagesSent.Any(p => p.Id == id) && !user.MessagesReceived.Any(p => p.Id == id))
                return Unauthorized();

            var messageToReturn = _mapper.To<MessageToReturnDto>(message);
            return Ok(messageToReturn);
        }

        // api/messages
        [HttpGet]
        public async Task<IActionResult> GetMessageForUser([FromQuery] MessageForFilterDto filterDto)
        {
            var messages = await _service.GetMessagesForUser(base.GetUserIdFromToken(), filterDto);

            Response.AddPagination(
                messages.CurrentPage,
                messages.PageSize,
                messages.TotalCount,
                messages.TotalPages
            );

            var messagesToReturn = _mapper.To<IEnumerable<MessageToReturnDto>>(messages);
            return Ok(messagesToReturn);
        }

        // api/messages/thread/{recipientId}
        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessagesThread(int recipientId)
        {
            var messages = await _service.GetMessagesThread(base.GetUserIdFromToken(), recipientId);

            var messagesToReturn = _mapper.To<IEnumerable<MessageToReturnDto>>(messages);
            return Ok(messagesToReturn);
        }

        // api/messages
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessageForCreationDto messageDto)
        {
            var message = await _service.SaveMessage(base.GetUserIdFromToken(), messageDto);

            var currentMessage = await _service.GetMessage(message.Id);

            var messageToReturn = _mapper.To<MessageToReturnDto>(currentMessage);
            return Ok(messageToReturn);
        }

        // api/messages/{id}/delete
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            await _service.DeleteMessage(id, base.GetUserIdFromToken());
            return NoContent();
        }

        // api/messages/thread/{recipientId}/mark-as-read
        [HttpPost("thread/{recipientId}/mark-as-read")]
        public async Task<IActionResult> MarkMessagesOfSenderAsRead(int recipientId)
        {
            await _service.MarkMessagesOfSenderAsRead(base.GetUserIdFromToken(), recipientId);
            return NoContent();
        }
    }
}