using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces.Services;
using DatingApp.Core.Exceptions;
using DatingApp.Core.Models;
using DatingApp.Core.Interfaces.Database;
using DatingApp.Core.Dtos.Messages;
using DatingApp.Core.Interfaces.Mappers;

namespace DatingApp.Infrastructure.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClassMapper _mapper;
        private readonly IUsersService _usersService;

        public MessagesService(IUnitOfWork unitOfWork, IClassMapper mapper, IUsersService usersService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public Task<Message> GetMessage(int id) =>
            _unitOfWork.Messages.GetMessage(id);

        public Task<Paginated<Message>> GetMessages(int userId, MessageForFilterDto filter) =>
             _unitOfWork.Messages.GetMessages(userId, filter);

        public Task<Paginated<Message>> GetMessagesThread(int userId, int recipientId) =>
            _unitOfWork.Messages.GetMessagesThread(userId, recipientId);

        public Task<IEnumerable<Message>> GetSenderMessagesThread(int userId, int recipientId) =>
            _unitOfWork.Messages.GetSenderMessagesThread(userId, recipientId);

        public async Task<Message> SaveMessage(int userId, MessageForCreationDto messageDto)
        {
            var recipient = await _usersService.GetUser(messageDto.RecipientId, false);
            if (recipient == null)
                throw new BadRequestException($"Could not find user id '{messageDto.RecipientId}'.");

            var message = _mapper.To<Message>(messageDto);
            message.SenderId = userId;

            _unitOfWork.Messages.Add(message);

            await _unitOfWork.CommitAsync();

            return message;
        }

        public async Task DeleteMessage(int messageId, int userId)
        {
            var message = await this.GetMessage(messageId);

            if (message.SenderId == userId)
                message.SenderDeleted = true;

            if (message.RecipientId == userId)
                message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
                _unitOfWork.Messages.Remove(message);

            await _unitOfWork.CommitAsync();
        }

        public async Task MarkMessagesOfSenderAsRead(int userId, int recipientId)
        {
            if (userId == recipientId)
                throw new UnauthorizedException();

            var messages = await this.GetSenderMessagesThread(userId, recipientId);

            foreach (var message in messages)
            {
                // security check
                if (message.RecipientId != userId)
                    throw new UnauthorizedException();
                if (message.SenderId != recipientId)
                    throw new UnauthorizedException();

                // only mark as read the unread messages
                if (message.IsRead == false)
                {
                    message.IsRead = true;
                    message.DateRead = DateTimeOffset.Now;
                }
            }

            await _unitOfWork.CommitAsync();
        }
    }
}