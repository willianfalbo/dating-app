using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Core.Dtos.Photos;
using DatingApp.Core.Entities;

namespace DatingApp.Core.Interfaces.Services
{
    public interface IPhotosService
    {
        Task<Photo> GetMainPhoto(int userId);
        Task<Photo> GetPhoto(int photoId);
        Task<Photo> UploadPhoto(int userId, PhotoForCreationDto userDto);
        Task SetMainPhoto(int userId, int photoId);
        Task DeletePhoto(int userId, int photoId);
        Task<IEnumerable<object>> GetPhotosForModeration();
        Task ApprovePhoto(int photoId);
        Task RejectPhoto(int photoId);
    }
}