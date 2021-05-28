using System;
using System.Threading.Tasks;
using DatingApp.Core.Entities;
using DatingApp.Core.Exceptions;
using DatingApp.Core.Interfaces.Database;
using DatingApp.Core.Interfaces.Services;

namespace DatingApp.Core.Services
{
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public LikeService(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task LikeUser(int id, int recipientId)
        {
            if (await _unitOfWork.Likes.GetLike(id, recipientId) != null)
                throw new BadRequestException("You already liked this user.");

            if (await _userService.GetUser(recipientId, false) == null)
                throw new NotFoundException($"User {recipientId} not found.");

            _unitOfWork.Likes.Add(new Like(id, recipientId));

            await _unitOfWork.CommitAsync();
        }
    }
}