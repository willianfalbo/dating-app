using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<User>> GetUsers() =>
            await _context.Users.Include(p => p.Photos).ToListAsync();

        public async Task<UserPhoto> GetUserPhoto(int userPhotoId) =>
            await _context.UserPhotos.FirstOrDefaultAsync(p => p.Id == userPhotoId);

        public async Task<UserPhoto> GetMainPhotoForUser(int userId) =>
            await _context.UserPhotos.FirstOrDefaultAsync(p => p.UserId == userId && p.IsMain);
    }
}