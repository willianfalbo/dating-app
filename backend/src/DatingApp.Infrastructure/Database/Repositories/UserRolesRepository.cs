using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Database.Repositories
{
    public class UserRolesRepository : Repository<UserRole>, IUserRolesRepository
    {
        public UserRolesRepository(DatabaseContext context) : base(context) { }

        public async Task<IEnumerable<object>> GetUsersWithRoles()
        {
            return await _context.Users
                .OrderBy(x => x.UserName)
                .Select(user => new
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = (from userRole in user.UserRoles
                             join role in _context.Roles
                                on userRole.RoleId equals role.Id
                             select role.Name).ToList()
                }).ToListAsync();
        }
    }
}