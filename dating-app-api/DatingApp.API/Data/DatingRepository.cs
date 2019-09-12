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

            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                query = query.Where(u => userLikers.Contains(u.Id));
            }

            if (userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                query = query.Where(u => userLikees.Contains(u.Id));
            }

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

        public async Task<Like> GetLike(int userId, int recipientId) =>
            await _context.Likes.FirstOrDefaultAsync(u =>
                u.LikerId == userId && u.LikeeId == recipientId);

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            var query = await _context.Users
                .Include(u => u.Likers)
                .Include(u => u.Likees)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (likers)
                return query.Likers.Where(u => u.LikeeId == id).Select(i => i.LikerId);
            else
                return query.Likees.Where(u => u.LikerId == id).Select(i => i.LikeeId);
        }

        public async Task<Message> GetMessage(int id) =>
            await _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .AsQueryable();

            switch (messageParams?.Container?.ToLower())
            {
                case "inbox":
                    query = query.Where(u => u.RecipientId == messageParams.UserId
                        && u.RecipientDeleted == false);
                    break;
                case "outbox":
                    query = query.Where(u => u.SenderId == messageParams.UserId
                        && u.SenderDeleted == false);
                    break;
                // unread
                default:
                    query = query.Where(u => u.RecipientId == messageParams.UserId 
                        && u.RecipientDeleted == false && u.IsRead == false);
                    break;
            }

            query = query.OrderByDescending(d => d.MessageSent);

            return await PagedList<Message>.CreateAsync(query,
                messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessagesThread(int userId, int recipientId) =>
            await _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(m => m.RecipientId == userId && m.SenderId == recipientId && m.RecipientDeleted == false
                         || m.RecipientId == recipientId && m.SenderId == userId && m.SenderDeleted == false)
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();

        public async Task<IEnumerable<Message>> GetSenderMessagesThread(int userId, int recipientId) =>
            await _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(m => m.RecipientId == userId && m.SenderId == recipientId && m.RecipientDeleted == false)
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();
    }
}