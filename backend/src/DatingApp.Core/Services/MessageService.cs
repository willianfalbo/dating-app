using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces.Services;
using DatingApp.Core.Exceptions;
using DatingApp.Core.Models;
using DatingApp.Core.Interfaces.Database;
using DatingApp.Core.Interfaces;
using DatingApp.Core.Dtos.Messages;

namespace DatingApp.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClassMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork, IClassMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<Message> GetMessage(int id) =>
            _unitOfWork.Messages.GetMessage(id);

        public Task<PagedResult<Message>> GetMessagesForUser(MessageForFilterDto filter) =>
             _unitOfWork.Messages.GetMessagesForUser(filter);

        public Task<IEnumerable<Message>> GetMessagesThread(int userId, int recipientId) =>
            _unitOfWork.Messages.GetMessagesThread(userId, recipientId);

        public Task<IEnumerable<Message>> GetSenderMessagesThread(int userId, int recipientId) =>
            _unitOfWork.Messages.GetSenderMessagesThread(userId, recipientId);

        public async Task<Message> SaveMessage(MessageForCreationDto messageDto)
        {
            var message = _mapper.To<Message>(messageDto);

            _unitOfWork.Messages.Add(message);

            await _unitOfWork.CommitAsync();

            return message;
        }

        public async Task DeleteMessage(int messageId, int userId)
        {
            var messageFromRepo = await this.GetMessage(messageId);

            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                _unitOfWork.Messages.Remove(messageFromRepo);

            await _unitOfWork.CommitAsync();
        }

        public async Task MarkMessagesOfSenderAsRead(int userId, int recipientId)
        {
            if (userId == recipientId)
                throw new UnauthorizedException();

            var messagesFromRepo = await this.GetSenderMessagesThread(userId, recipientId);

            foreach (var message in messagesFromRepo)
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