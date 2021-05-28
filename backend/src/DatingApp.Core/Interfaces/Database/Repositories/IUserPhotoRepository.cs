using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Core.Entities;

namespace DatingApp.Core.Interfaces.Database.Repositories
{
    public interface IUserPhotoRepository : IRepository<UserPhoto>
    {
        Task<UserPhoto> GetMainPhotoForUser(int userId);
        Task<UserPhoto> GetUserPhoto(int photoId);
        Task<IEnumerable<object>> GetPhotosForModeration();
    }
}