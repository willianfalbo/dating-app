using System;
using System.Threading.Tasks;
using DatingApp.Core.Interfaces.Database;
using DatingApp.Core.Interfaces.Database.Repositories;
using DatingApp.Infrastructure.Database.Repositories;

namespace DatingApp.Infrastructure.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            Likes = new LikeRepository(_context);
            Messages = new MessageRepository(_context);
            UserPhotos = new UserPhotoRepository(_context);
            Users = new UserRepository(_context);
            UserRoles = new UserRolesRepository(_context);
        }

        public ILikeRepository Likes { get; }
        public IMessageRepository Messages { get; }
        public IUserPhotoRepository UserPhotos { get; }
        public IUserRepository Users { get; }
        public IUserRolesRepository UserRoles { get; }

        public async Task<bool> CommitAsync() =>
             await _context.SaveChangesAsync() >= 0;

        public async ValueTask DisposeAsync() =>
            await _context.DisposeAsync();
    }
}