using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    // [Authorize] // once we are using AspNet Core Identity, we dont need this line anymore
    [Route("api/users")]
    [ApiController]
    public class UserController : CustomControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public UserController(IDatingRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        // api/users
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var currentUserFromRepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            var usersFromRepo = await _repo.GetUsers(userParams);
            var usersForListDto = _mapper.Map<IEnumerable<UserForListDto>>(usersFromRepo);

            Response.AddPagination(usersFromRepo.CurrentPage, usersFromRepo.PageSize,
                usersFromRepo.TotalCount, usersFromRepo.TotalPages);

            return Ok(usersForListDto);
        }

        // api/users/{id}
        [HttpGet("{id}", Name = nameof(GetUser))]
        public async Task<IActionResult> GetUser(int id)
        {
            var userFromRepo = await _repo.GetUser(id);
            var user = _mapper.Map<UserForDetailedDto>(userFromRepo);
            return Ok(user);
        }

        // api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]UserForUpdateDto userForUpdateDto)
        {
            if (!base.DoesUserMatchWithToken(id))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll())
                return Ok(_mapper.Map<UserForDetailedDto>(userFromRepo));

            return BadRequest($"Updating user {id} failed on save");
        }

        // api/users/{userId}/like/{recipientId}
        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (!base.DoesUserMatchWithToken(id))
                return Unauthorized();

            if (await _repo.GetLike(id, recipientId) != null)
                return BadRequest("You already liked this user");

            if (await _repo.GetUser(recipientId) == null)
                return NotFound();

            var like = new Like(id, recipientId);

            _repo.Add<Like>(like);

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to like user");
        }
    }
}