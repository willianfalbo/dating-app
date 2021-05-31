using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Api.Helpers;
using DatingApp.Core.Dtos.Users;
using DatingApp.Core.Interfaces;
using DatingApp.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/users")]
    [ApiController]
    public class UsersController : CustomControllerBase
    {
        private readonly IUserService _service;
        private readonly ILikeService _likeService;
        private readonly IClassMapper _mapper;

        public UsersController(IUserService service, ILikeService likeService, IClassMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _likeService = likeService ?? throw new ArgumentNullException(nameof(likeService));
        }

        // api/users
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserForFilterDto filterDto)
        {
            var currentUserId = base.GetUserIdFromToken();

            var currentUserFromRepo = await _service.GetUser(currentUserId, true);

            filterDto.UserId = currentUserId;

            var usersFromRepo = await _service.GetUsers(filterDto);
            var usersForListDto = _mapper.To<IEnumerable<UserForListDto>>(usersFromRepo);

            Response.AddPagination(usersFromRepo.CurrentPage, usersFromRepo.PageSize,
                usersFromRepo.TotalCount, usersFromRepo.TotalPages);

            return Ok(usersForListDto);
        }

        // api/users/{id}
        [HttpGet("{id}", Name = nameof(GetUser))]
        public async Task<IActionResult> GetUser(int id)
        {
            var isCurrentUser = base.GetUserIdFromToken() == id;

            var userFromRepo = await _service.GetUser(id, isCurrentUser);
            var user = _mapper.To<UserForDetailedDto>(userFromRepo);
            return Ok(user);
        }

        // api/users
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserForUpdateDto userDto)
        {
            var user = await _service.UpdateUser(base.GetUserIdFromToken(), userDto);
            return Ok(_mapper.To<UserForDetailedDto>(user));
        }

        // api/users/{recipientId}/like
        [HttpPost("{recipientId}/like")]
        public async Task<IActionResult> LikeUser(int recipientId)
        {
            await _likeService.LikeUser(base.GetUserIdFromToken(), recipientId);
            return NoContent();
        }
    }
}