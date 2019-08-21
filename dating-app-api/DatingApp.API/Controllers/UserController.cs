using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> GetUsers()
        {
            var usersFromRepo = await _repo.GetUsers();
            var usersForListDto = _mapper.Map<IEnumerable<UserForListDto>>(usersFromRepo);
            return Ok(usersForListDto);
        }

        // api/users/6
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userFromRepo = await _repo.GetUser(id);
            var user = _mapper.Map<UserForDetailedDto>(userFromRepo);
            return Ok(user);
        }

        // api/users/6
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]UserForUpdateDto userForUpdateDto)
        {
            if (!base.DoesUserMatchWithToken(id))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll())
                return Ok(_mapper.Map<UserForDetailedDto>(userFromRepo));

            throw new Exception($"Updating user {id} failed on save");
        }
    }
}