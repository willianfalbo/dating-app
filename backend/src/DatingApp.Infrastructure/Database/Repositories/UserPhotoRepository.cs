using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Database.Repositories
{
    public class UserPhotoRepository : Repository<UserPhoto>, IUserPhotoRepository
    {
        public UserPhotoRepository(DatabaseContext context) : base(context) { }

        public Task<UserPhoto> GetMainPhotoForUser(int userId) =>
            _context.UserPhotos
                .FirstOrDefaultAsync(p => p.UserId == userId && p.IsMain);

        public Task<UserPhoto> GetUserPhoto(int photoId) =>
            _context.UserPhotos
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == photoId);

        public async Task<IEnumerable<object>> GetPhotosForModeration()
        {
            return await _context.UserPhotos
                .Include(u => u.User)
                .IgnoreQueryFilters()
                .Where(p => p.IsApproved == false)
                .Select(u => new
                {
                    Id = u.Id,
                    UserName = u.User.UserName,
                    Url = u.Url,
                    IsApproved = u.IsApproved
                }).ToListAsync();
        }
    }
}