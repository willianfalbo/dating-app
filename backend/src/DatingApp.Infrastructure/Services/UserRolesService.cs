using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Core.Interfaces.Database;
using DatingApp.Core.Interfaces.Mappers;
using DatingApp.Core.Interfaces.Services;

namespace DatingApp.Infrastructure.Services
{
    public class UserRolesService : IUserRolesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClassMapper _mapper;

        public UserRolesService(IUnitOfWork unitOfWork, IClassMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<IEnumerable<object>> GetUsersWithRoles() =>
            _unitOfWork.UserRoles.GetUsersWithRoles();
    }
}