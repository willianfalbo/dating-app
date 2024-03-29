using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Core.Dtos.Likes;
using DatingApp.Core.Dtos.Users;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces.Database.Repositories;
using DatingApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Database.Repositories
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public UsersRepository(DatabaseContext context) : base(context) { }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<User> GetUser(int id, bool isCurrentUser)
        {
            var query = _context.Users.Include(p => p.Photos).AsQueryable();

            if (isCurrentUser)
                query = query.IgnoreQueryFilters();

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<Paginated<User>> GetUsers(UserForFilterDto filter)
        {
            var query = _context.Users
                .Include(p => p.Photos)
                .OrderByDescending(u => u.LastActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Gender))
                query = query.Where(u => u.Gender == filter.Gender);

            if (filter.MinAge != 18 || filter.MaxAge != 99)
            {
                var minDateOfBirth = DateTime.Today.AddYears(-filter.MaxAge - 1);
                var maxDateOfBirth = DateTime.Today.AddYears(-filter.MinAge);

                query = query.Where(u => u.DateOfBirth >= minDateOfBirth && u.DateOfBirth <= maxDateOfBirth);
            }

            if (!string.IsNullOrWhiteSpace(filter.OrderBy))
            {
                switch (filter.OrderBy.ToLower())
                {
                    case "created":
                        query = query.OrderByDescending(u => u.Created);
                        break;
                    default:
                        query = query.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await this.PaginatedFilterAsync(query, filter.Page, filter.Limit);
        }

        public async Task<Paginated<User>> GetUserLikes(int userId, LikeForFilterDto filter)
        {
            var query = _context.Users
                .Include(p => p.Photos)
                .OrderByDescending(u => u.LastActive)
                .AsQueryable();

            query = query.Where(u => u.Id != userId);

            var likeIds = await GetIdsUserLikes(userId, filter.FilterSender);
            query = query.Where(u => likeIds.Contains(u.Id));

            return await this.PaginatedFilterAsync(query, filter.Page, filter.Limit);
        }

        private async Task<IEnumerable<int>> GetIdsUserLikes(int userId, bool filterSender = true)
        {
            var query = await _context.Users
                .Include(u => u.LikesSent)
                .Include(u => u.LikesReceived)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (filterSender)
                return query.LikesSent.Where(u => u.ReceiverId == userId).Select(i => i.SenderId);
            else
                return query.LikesReceived.Where(u => u.SenderId == userId).Select(i => i.ReceiverId);
        }
    }
}