using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Api.Helpers;
using DatingApp.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using DatingApp.Core.Dtos.Messages;
using DatingApp.Core.Interfaces.Mappers;
using DatingApp.Core.Models;

namespace DatingApp.Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/messages")]
    [ApiController]
    public class MessagesController : CustomControllerBase
    {
        private readonly IMessagesService _service;
        private readonly IUsersService _usersService;
        private readonly IClassMapper _mapper;

        public MessagesController(IMessagesService service, IUsersService usersService, IClassMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // api/messages/{id}
        [HttpGet("{id}", Name = nameof(GetMessage))]
        public async Task<IActionResult> GetMessage(int id)
        {
            var message = await _service.GetMessage(id);
            if (message == null)
                return NotFound();

            var user = await _usersService.GetUser(base.GetUserIdFromToken(), true);
            if (!user.MessagesSent.Any(p => p.Id == id) && !user.MessagesReceived.Any(p => p.Id == id))
                return Unauthorized();

            return Ok(_mapper.To<MessageToReturnDto>(message));
        }

        // api/messages
        [HttpGet]
        public async Task<IActionResult> GetMessageForUser([FromQuery] MessageForFilterDto filterDto)
        {
            var messages = await _service.GetMessages(base.GetUserIdFromToken(), filterDto);

            return Ok(_mapper.To<Paginated<MessageToReturnDto>>(messages));
        }

        // api/messages/thread/{recipientId}
        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessagesThread(int recipientId)
        {
            var messages = await _service.GetMessagesThread(base.GetUserIdFromToken(), recipientId);
            return Ok(_mapper.To<Paginated<MessageToReturnDto>>(messages));
        }

        // api/messages
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessageForCreationDto messageDto)
        {
            var message = await _service.SaveMessage(base.GetUserIdFromToken(), messageDto);

            var currentMessage = await _service.GetMessage(message.Id);

            return Ok(_mapper.To<MessageToReturnDto>(currentMessage));
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