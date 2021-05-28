using System.Threading.Tasks;
using DatingApp.Core.Dtos.Users;
using DatingApp.Core.Entities;
using DatingApp.Core.Models;

namespace DatingApp.Core.Interfaces.Database.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByUsername(string username);
        Task<User> GetUser(int id, bool isCurrentUser);
        Task<PagedResult<User>> GetUsers(UserForFilterDto filter);
    }
}