using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Core.Entities;

namespace DatingApp.Core.Interfaces.Database.Repositories
{
    public interface IUserRolesRepository : IRepository<UserRole>
    {
        Task<IEnumerable<object>> GetUsersWithRoles();
    }
}