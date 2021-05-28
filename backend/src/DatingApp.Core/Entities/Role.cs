using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Core.Entities
{
    // TODO: Move identity to infrastructure project
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}