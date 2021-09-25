using System;
using System.Threading.Tasks;
using DatingApp.Core.Dtos.Users;
using DatingApp.Core.Entities;
using DatingApp.Core.Interfaces.Database;
using DatingApp.Core.Interfaces.Mappers;
using DatingApp.Core.Interfaces.Services;
using DatingApp.Core.Models;

namespace DatingApp.Infrastructure.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClassMapper _mapper;

        public UsersService(IUnitOfWork unitOfWork, IClassMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<User> GetUser(int id, bool isCurrentUser) =>
            _unitOfWork.Users.GetUser(id, isCurrentUser);

        public Task<User> GetUserByUsername(string username) =>
             _unitOfWork.Users.GetUserByUsername(username);

        public Task<Paginated<User>> GetUsers(UserForFilterDto filter) =>
            _unitOfWork.Users.GetUsers(filter);

        public async Task<User> UpdateUser(int id, UserForUpdateDto userDto)
        {
            var user = await this.GetUser(id, true);

            _mapper.FromTo(userDto, user);

            await _unitOfWork.CommitAsync();

            return user;
        }

        public async Task UpdateUserActivity(int id)
        {
            var user = await this.GetUser(id, true);
            user.LastActive = DateTimeOffset.Now;

            await _unitOfWork.CommitAsync();
        }
    }
}