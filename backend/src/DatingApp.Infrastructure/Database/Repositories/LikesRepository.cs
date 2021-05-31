using System.Threading.Tasks;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Database.Repositories
{
    public class LikesRepository : Repository<Like>, ILikesRepository
    {
        public LikesRepository(DatabaseContext context) : base(context) { }

        public async Task<Like> GetLike(int userId, int recipientId) =>
            await _context.Likes
                .Include(p => p.Receiver)
                .FirstOrDefaultAsync(u =>
                u.SenderId == userId && u.ReceiverId == recipientId);
    }
}