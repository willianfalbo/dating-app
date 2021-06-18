using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Database.Repositories
{
    public class PhotosRepository : Repository<Photo>, IPhotosRepository
    {
        public PhotosRepository(DatabaseContext context) : base(context) { }

        public Task<Photo> GetMainPhoto(int userId) =>
            _context.Photos
                .FirstOrDefaultAsync(p => p.UserId == userId && p.IsMain);

        public Task<Photo> GetPhoto(int photoId) =>
            _context.Photos
                .IgnoreQueryFilters()
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == photoId);

        public async Task<IEnumerable<object>> GetPhotosForModeration()
        {
            return await _context.Photos
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