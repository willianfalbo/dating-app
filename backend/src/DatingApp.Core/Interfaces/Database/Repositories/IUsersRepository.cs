using System.Threading.Tasks;
using DatingApp.Core.Dtos.Likes;
using DatingApp.Core.Dtos.Users;
using DatingApp.Core.Entities;
using DatingApp.Core.Models;

namespace DatingApp.Core.Interfaces.Database.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<User> GetUserByUsername(string username);
        Task<User> GetUser(int id, bool isCurrentUser);
        Task<PagedResult<User>> GetUserLikes(int userId, LikeForFilterDto filter);
        Task<PagedResult<User>> GetUsers(int userId, UserForFilterDto filter);
    }
}