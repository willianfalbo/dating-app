using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Core.Dtos.Messages;
using DatingApp.Core.Entities;
using DatingApp.Core.Models;

namespace DatingApp.Core.Interfaces.Services
{
    public interface IMessagesService
    {
        Task<Message> GetMessage(int id);
        Task<Paginated<Message>> GetMessages(int userId, MessageForFilterDto filter);
        Task<Paginated<Message>> GetMessagesThread(int userId, int recipientId);
        Task<IEnumerable<Message>> GetSenderMessagesThread(int userId, int recipientId);
        Task<Message> SaveMessage(int userId, MessageForCreationDto messageDto);
        Task DeleteMessage(int messageId, int userId);
        Task MarkMessagesOfSenderAsRead(int userId, int recipientId);
    }
}