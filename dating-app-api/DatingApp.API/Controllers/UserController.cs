using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
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
        public async Task<IActionResult> GetUsers()
        {
            var usersData = await _repo.GetUsers();
            var users = _mapper.Map<IEnumerable<UserForListDto>>(usersData);
            return Ok(users);
        }

        // api/users/6
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userData = await _repo.GetUser(id);
            var user = _mapper.Map<UserForDetailedDto>(userData);
            return Ok(user);
        }

        // api/users/6
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userDto)
        {
            // check if the user id parameter matches with the token id
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userData = await _repo.GetUser(id);

            _mapper.Map(userDto, userData);

            if (await _repo.SaveAll())
                return Ok(_mapper.Map<UserForDetailedDto>(userData));

            throw new Exception($"Updating user {id} failed on save");
        }
    }
}