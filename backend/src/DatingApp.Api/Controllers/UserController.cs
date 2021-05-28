using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Api.Helpers;
using DatingApp.Core.Dtos;
using DatingApp.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    // [Authorize] // once we are using AspNet Core Identity, we dont need this line anymore
    [Route("api/users")]
    [ApiController]
    public class UserController : CustomControllerBase
    {
        private readonly IUserService _service;
        private readonly ILikeService _likeService;
        private readonly IMapper _mapper;

        public UserController(IUserService service, ILikeService likeService, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _likeService = likeService ?? throw new ArgumentNullException(nameof(likeService));
        }

        // api/users
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserFilterDto filterDto)
        {
            var currentUserId = base.GetUserId();

            var currentUserFromRepo = await _service.GetUser(currentUserId, true);

            filterDto.UserId = currentUserId;

            var usersFromRepo = await _service.GetUsers(filterDto);
            var usersForListDto = _mapper.Map<IEnumerable<UserForListDto>>(usersFromRepo);

            Response.AddPagination(usersFromRepo.CurrentPage, usersFromRepo.PageSize,
                usersFromRepo.TotalCount, usersFromRepo.TotalPages);

            return Ok(usersForListDto);
        }

        // api/users/{id}
        [HttpGet("{id}", Name = nameof(GetUser))]
        public async Task<IActionResult> GetUser(int id)
        {
            var isCurrentUser = base.GetUserId() == id;

            var userFromRepo = await _service.GetUser(id, isCurrentUser);
            var user = _mapper.Map<UserForDetailedDto>(userFromRepo);
            return Ok(user);
        }

        // api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserForUpdateDto userDto)
        {
            if (!base.DoesUserMatchWithToken(id))
                return Unauthorized();

            var user = await _service.UpdateUser(id, userDto);

            return Ok(_mapper.Map<UserForDetailedDto>(user));
        }

        // api/users/{userId}/like/{recipientId}
        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (!base.DoesUserMatchWithToken(id))
                return Unauthorized();

            await _likeService.LikeUser(id, recipientId);

            return NoContent();
        }
    }
}