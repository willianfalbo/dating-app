using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Core.Dtos;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces.Database.Repositories;
using DatingApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Database.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(DatabaseContext context) : base(context) { }

        public async Task<Message> GetMessage(int id) =>
            await _context.Messages
                // .Include(u => u.Sender).ThenInclude(p => p.Photos)
                // .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<PagedResult<Message>> GetMessagesForUser(MessageFilterDto filter)
        {
            var query = _context.Messages
                // .Include(u => u.Sender).ThenInclude(p => p.Photos)
                // .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .AsQueryable();

            switch (filter.Container?.ToLower())
            {
                case "inbox":
                    query = query.Where(u => u.RecipientId == filter.UserId
                        && u.RecipientDeleted == false);
                    break;
                case "outbox":
                    query = query.Where(u => u.SenderId == filter.UserId
                        && u.SenderDeleted == false);
                    break;
                // unread
                default:
                    query = query.Where(u => u.RecipientId == filter.UserId
                        && u.RecipientDeleted == false && u.IsRead == false);
                    break;
            }

            query = query.OrderByDescending(d => d.MessageSent);

            return await FilterAsync(query, filter.PageNumber, filter.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessagesThread(int userId, int recipientId) =>
            await _context.Messages
                // .Include(u => u.Sender).ThenInclude(p => p.Photos)
                // .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(m => m.RecipientId == userId && m.SenderId == recipientId && m.RecipientDeleted == false
                         || m.RecipientId == recipientId && m.SenderId == userId && m.SenderDeleted == false)
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();

        public async Task<IEnumerable<Message>> GetSenderMessagesThread(int userId, int recipientId) =>
            await _context.Messages
                // .Include(u => u.Sender).ThenInclude(p => p.Photos)
                // .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(m => m.RecipientId == userId && m.SenderId == recipientId && m.RecipientDeleted == false)
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();
    }
}