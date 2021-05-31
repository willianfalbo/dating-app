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
        private readonly IUsersService _service;
        private readonly ILikesService _likesService;
        private readonly IClassMapper _mapper;

        public UsersController(IUsersService service, ILikesService likesService, IClassMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _likesService = likesService ?? throw new ArgumentNullException(nameof(likesService));
        }

        // api/users
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserForFilterDto filterDto)
        {
            var users = await _service.GetUsers(base.GetUserIdFromToken(), filterDto);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(_mapper.To<IEnumerable<UserForListDto>>(users));
        }

        // api/users/{id}
        [HttpGet("{id}", Name = nameof(GetUser))]
        public async Task<IActionResult> GetUser(int id)
        {
            var isCurrentUser = base.GetUserIdFromToken() == id;
            var user = await _service.GetUser(id, isCurrentUser);

            return Ok(_mapper.To<UserForDetailedDto>(user));
        }

        // api/users
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserForUpdateDto userDto)
        {
            var user = await _service.UpdateUser(base.GetUserIdFromToken(), userDto);
            return Ok(_mapper.To<UserForDetailedDto>(user));
        }
    }
}