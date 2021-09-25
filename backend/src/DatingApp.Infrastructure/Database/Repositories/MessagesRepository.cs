using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Core.Dtos.Messages;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces.Database.Repositories;
using DatingApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Database.Repositories
{
    public class MessagesRepository : Repository<Message>, IMessagesRepository
    {
        public MessagesRepository(DatabaseContext context) : base(context) { }

        public async Task<Message> GetMessage(int id) =>
            await _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<Paginated<Message>> GetMessages(int userId, MessageForFilterDto filter)
        {
            var query = _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .AsQueryable();

            switch (filter.Container?.ToLower())
            {
                case "inbox":
                    query = query.Where(u => u.RecipientId == userId
                        && u.RecipientDeleted == false);
                    break;
                case "outbox":
                    query = query.Where(u => u.SenderId == userId
                        && u.SenderDeleted == false);
                    break;
                default:
                    query = query.Where(u => u.RecipientId == userId
                        && u.RecipientDeleted == false && u.IsRead == false);
                    break;
            }

            query = query.OrderByDescending(d => d.MessageSent);

            return await this.PaginatedFilterAsync(query, filter.Page, filter.Limit);
        }

        public async Task<Paginated<Message>> GetMessagesThread(int userId, int recipientId) =>
            await this.PaginatedFilterAsync(
                filter: m =>
                    m.RecipientId == userId && m.SenderId == recipientId && m.RecipientDeleted == false ||
                    m.RecipientId == recipientId && m.SenderId == userId && m.SenderDeleted == false,
                include: m =>
                    m.Include(p => p.Sender).ThenInclude(p => p.Photos)
                    .Include(p => p.Recipient).ThenInclude(p => p.Photos),
                ordination: m =>
                    m.OrderBy(p => p.MessageSent),
                page: 1,
                limit: 100 // TODO: Add load more pagination in frontend
            );

        public async Task<IEnumerable<Message>> GetSenderMessagesThread(int userId, int recipientId) =>
            await _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(m => m.RecipientId == userId && m.SenderId == recipientId && m.RecipientDeleted == false)
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();
    }
}