using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Core.Dtos.Users;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces.Database.Repositories;
using DatingApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Database.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context) { }

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

        public async Task<PagedResult<User>> GetUsers(UserForFilterDto filter)
        {
            var query = _context.Users
                .Include(p => p.Photos)
                .OrderByDescending(u => u.LastActive)
                .AsQueryable();

            query = query.Where(u => u.Id != filter.UserId);

            if (!string.IsNullOrWhiteSpace(filter.Gender))
                query = query.Where(u => u.Gender == filter.Gender);

            if (filter.Likers)
            {
                var userLikers = await GetUserLikes(filter.UserId, filter.Likers);
                query = query.Where(u => userLikers.Contains(u.Id));
            }

            if (filter.Likees)
            {
                var userLikees = await GetUserLikes(filter.UserId, filter.Likers);
                query = query.Where(u => userLikees.Contains(u.Id));
            }

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

            return await this.PagedFilterAsync(query, filter.PageNumber, filter.PageSize);
        }

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            var query = await _context.Users
                .Include(u => u.LikesSent)
                .Include(u => u.LikesReceived)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (likers)
                return query.LikesSent.Where(u => u.ReceiverId == id).Select(i => i.SenderId);
            else
                return query.LikesReceived.Where(u => u.SenderId == id).Select(i => i.ReceiverId);
        }
    }
}