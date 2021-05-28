using System.Threading.Tasks;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Database.Repositories
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        public LikeRepository(DatabaseContext context) : base(context) { }

        public async Task<Like> GetLike(int userId, int recipientId) =>
            await _context.Likes.FirstOrDefaultAsync(u =>
                u.SenderId == userId && u.ReceiverId == recipientId);
    }
}