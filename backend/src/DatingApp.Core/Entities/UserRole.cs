using Microsoft.AspNetCore.Identity;

namespace DatingApp.Core.Entities
{
    // TODO: Move identity to infrastructure project
    public class UserRole : IdentityUserRole<int>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}