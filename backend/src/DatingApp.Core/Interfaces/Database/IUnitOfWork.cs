using System;
using System.Threading.Tasks;
using DatingApp.Core.Interfaces.Database.Repositories;

namespace DatingApp.Core.Interfaces.Database
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        ILikesRepository Likes { get; }
        IMessagesRepository Messages { get; }
        IPhotosRepository Photos { get; }
        IUsersRepository Users { get; }
        IUserRolesRepository UserRoles { get; }

        Task<bool> CommitAsync();
    }
}