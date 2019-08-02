using System.Collections.Generic;
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
    }
}