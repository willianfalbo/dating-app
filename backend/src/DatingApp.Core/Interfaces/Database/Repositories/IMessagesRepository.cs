using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Core.Dtos.Messages;
using DatingApp.Core.Entities;
using DatingApp.Core.Models;

namespace DatingApp.Core.Interfaces.Database.Repositories
{
    public interface IMessagesRepository : IRepository<Message>
    {
        Task<Message> GetMessage(int id);
        Task<Paginated<Message>> GetMessages(int userId, MessageForFilterDto filter);
        Task<Paginated<Message>> GetMessagesThread(int userId, int recipientId);
        Task<IEnumerable<Message>> GetSenderMessagesThread(int userId, int recipientId);
    }
}