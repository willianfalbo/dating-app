using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Core.Dtos;
using DatingApp.Core.Entities;

namespace DatingApp.Core.Interfaces.Services
{
    public interface IUserPhotoService
    {
        Task<UserPhoto> GetMainPhotoForUser(int userId);
        Task<UserPhoto> GetUserPhoto(int photoId);
        Task<UserPhoto> UploadUserPhoto(int userId, UserPhotoForCreationDto userDto);
        Task SetMainPhoto(int userId, int userPhotoId);
        Task DeletePhoto(int userId, int userPhotoId);
        Task<IEnumerable<object>> GetPhotosForModeration();
        Task ApprovePhoto(int photoId);
        Task RejectPhoto(int photoId);
    }
}