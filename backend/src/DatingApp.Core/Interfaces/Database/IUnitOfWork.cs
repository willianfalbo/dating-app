using System;
using System.Threading.Tasks;
using DatingApp.Core.Interfaces.Database.Repositories;

namespace DatingApp.Core.Interfaces.Database
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        ILikeRepository Likes { get; }
        IMessageRepository Messages { get; }
        IUserPhotoRepository UserPhotos { get; }
        IUserRepository Users { get; }
        IUserRolesRepository UserRoles { get; }

        Task<bool> CommitAsync();
    }
}