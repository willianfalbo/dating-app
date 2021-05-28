using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Core.Dtos;
using DatingApp.Core.Entities;
using DatingApp.Core.Models;

namespace DatingApp.Core.Interfaces.Database.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<Message> GetMessage(int id);
        Task<PagedResult<Message>> GetMessagesForUser(MessageFilterDto filter);
        Task<IEnumerable<Message>> GetMessagesThread(int userId, int recipientId);
        Task<IEnumerable<Message>> GetSenderMessagesThread(int userId, int recipientId);
    }
}