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

            Likes = new LikesRepository(_context);
            Messages = new MessagesRepository(_context);
            Photos = new PhotosRepository(_context);
            Users = new UsersRepository(_context);
            UserRoles = new UserRolesRepository(_context);
        }

        public ILikesRepository Likes { get; }
        public IMessagesRepository Messages { get; }
        public IPhotosRepository Photos { get; }
        public IUsersRepository Users { get; }
        public IUserRolesRepository UserRoles { get; }

        public async Task<bool> CommitAsync() =>
             await _context.SaveChangesAsync() >= 0;

        public async ValueTask DisposeAsync() =>
            await _context.DisposeAsync();
    }
}