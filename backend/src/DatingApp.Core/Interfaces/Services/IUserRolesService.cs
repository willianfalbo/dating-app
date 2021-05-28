using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.Core.Interfaces.Services
{
    public interface IUserRolesService
    {
        Task<IEnumerable<object>> GetUsersWithRoles();
    }
}