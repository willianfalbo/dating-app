using System.Threading.Tasks;
using DatingApp.Core.Dtos.Users;
using DatingApp.Core.Entities;
using DatingApp.Core.Models;

namespace DatingApp.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> GetUserByUsername(string username);
        Task<User> GetUser(int id, bool isCurrentUser);
        Task<PagedResult<User>> GetUsers(UserForFilterDto filter);
        Task<User> UpdateUser(int id, UserForUpdateDto userDto);
        Task UpdateUserActivity(int id);
    }
}