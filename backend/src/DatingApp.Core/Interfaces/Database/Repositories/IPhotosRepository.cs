using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Core.Entities;

namespace DatingApp.Core.Interfaces.Database.Repositories
{
    public interface IPhotosRepository : IRepository<Photo>
    {
        Task<Photo> GetMainPhoto(int userId);
        Task<Photo> GetPhoto(int photoId);
        Task<IEnumerable<object>> GetPhotosForModeration();
    }
}