using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DatingApp.Core.Constants;
using DatingApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DatingApp.Infrastructure.Database.Seeds
{
    // This class was written using JsonGenerator and RandomUser
    // https://www.json-generator.com/
    // https://randomuser.me/
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                CreateAppRoles(roleManager);
                CreateAdminUser(userManager);
                CreateUsers(userManager);
            }
        }

        private static void CreateAppRoles(RoleManager<Role> roleManager)
        {
            var roles = new List<Role>
                {
                    new Role { Name = Roles.Member },
                    new Role { Name = Roles.Admin },
                    new Role { Name = Roles.Moderator },
                };

            foreach (var role in roles)
            {
                roleManager.CreateAsync(role).Wait();
            }
        }

        private static void CreateAdminUser(UserManager<User> userManager)
        {
            var adminUser = new User { UserName = "Admin" };

            var result = userManager.CreateAsync(adminUser, "password").Result;

            if (result.Succeeded)
            {
                var admin = userManager.FindByNameAsync("Admin").Result;
                userManager.AddToRolesAsync(admin, new[] { Roles.Admin, Roles.Moderator }).Wait();
            }
        }

        private static void CreateUsers(UserManager<User> userManager)
        {
            string userSeedPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "DatingApp.Infrastructure", "Database", "Seeds", "UserSeedData.json");
            var userData = File.ReadAllText(userSeedPath);
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach (var user in users)
            {
                user.Photos.SingleOrDefault().IsApproved = true;
                userManager.CreateAsync(user, "password").Wait();
                userManager.AddToRoleAsync(user, Roles.Member).Wait();
            }
        }

        // This class was copied from AuthRepository.cs
        // It is only for testing purpose and we do not want to mess around
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
