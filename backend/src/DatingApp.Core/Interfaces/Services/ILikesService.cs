using System.Threading.Tasks;
using DatingApp.Core.Dtos.Likes;
using DatingApp.Core.Entities;
using DatingApp.Core.Models;

namespace DatingApp.Core.Interfaces.Services
{
    public interface ILikesService
    {
        Task AddLike(int userId, int recipientId);
        Task<PagedResult<User>> GetLikes(int userId, LikeForFilterDto filter);
    }
}