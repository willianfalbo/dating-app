using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            this._context = context;
        }

        public void Add<T>(T entity) where T : class
            => _context.Add(entity);

        public void Delete<T>(T entity) where T : class
            => _context.Remove(entity);

        public async Task<bool> SaveAll() =>
            await _context.SaveChangesAsync() >= 0;

        public async Task<User> GetUser(int id) =>
            await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var query = _context.Users.Include(p => p.Photos)
                .OrderByDescending(u => u.LastActive)
                .AsQueryable();

            query = query.Where(u => u.Id != userParams.UserId);
            query = query.Where(u => u.Gender == userParams.Gender);

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDateOfBirth = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDateOfBirth = DateTime.Today.AddYears(-userParams.MinAge);

                query = query.Where(u => u.DateOfBirth >= minDateOfBirth && u.DateOfBirth <= maxDateOfBirth);
            }

            if (!string.IsNullOrWhiteSpace(userParams.OrderBy))
            {
                switch (userParams.OrderBy.ToLower())
                {
                    case "created":
                        query = query.OrderByDescending(u => u.Created);
                        break;
                    default:
                        query = query.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<UserPhoto> GetUserPhoto(int userPhotoId) =>
            await _context.UserPhotos.FirstOrDefaultAsync(p => p.Id == userPhotoId);

        public async Task<UserPhoto> GetMainPhotoForUser(int userId) =>
            await _context.UserPhotos.FirstOrDefaultAsync(p => p.UserId == userId && p.IsMain);
    }
}