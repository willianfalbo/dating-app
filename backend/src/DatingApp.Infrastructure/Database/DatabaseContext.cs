using DatingApp.Core.Entities;
using DatingApp.Infrastructure.Database.Configs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Database
{
    public class DatabaseContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // public DbSet<User> Users { get; set; } // once we are using IdentityDbContext, we don't need this line anymore
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // map tables
            builder.ApplyConfiguration(new UserRoleMap());
            builder.ApplyConfiguration(new LikeMap());
            builder.ApplyConfiguration(new MessageMap());
            builder.ApplyConfiguration(new PhotoMap());
        }
    }
}
