using System;
using System.Threading.Tasks;
using DatingApp.Core.Dtos.Likes;
using DatingApp.Core.Entities;
using DatingApp.Core.Exceptions;
using DatingApp.Core.Interfaces.Database;
using DatingApp.Core.Interfaces.Services;
using DatingApp.Core.Models;

namespace DatingApp.Infrastructure.Services
{
    public class LikesService : ILikesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersService _usersService;

        public LikesService(IUnitOfWork unitOfWork, IUsersService usersService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public async Task AddLike(int userId, int receiverId)
        {
            var like = await _unitOfWork.Likes.GetLike(userId, receiverId);
            if (like != null)
                throw new ConflictException($"You already liked '{like.Receiver.UserName}'.");

            if (await _usersService.GetUser(receiverId, false) == null)
                throw new NotFoundException($"User id '{receiverId}' was not found.");

            _unitOfWork.Likes.Add(new Like(userId, receiverId));

            await _unitOfWork.CommitAsync();
        }

        public Task<Paginated<User>> GetLikes(int userId, LikeForFilterDto filter) =>
            _unitOfWork.Users.GetUserLikes(userId, filter);
    }
}