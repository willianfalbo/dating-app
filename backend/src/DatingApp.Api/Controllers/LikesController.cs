using System;
using System.Threading.Tasks;
using DatingApp.Api.Helpers;
using DatingApp.Core.Dtos.Likes;
using DatingApp.Core.Dtos.Users;
using DatingApp.Core.Interfaces.Mappers;
using DatingApp.Core.Interfaces.Services;
using DatingApp.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/likes")]
    [ApiController]
    public class LikesController : CustomControllerBase
    {
        private readonly ILikesService _service;
        private readonly IClassMapper _mapper;

        public LikesController(ILikesService service, IClassMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // api/likes
        [HttpGet]
        public async Task<IActionResult> GetUserLikes([FromQuery] LikeForFilterDto filterDto)
        {
            var users = await _service.GetLikes(base.GetUserIdFromToken(), filterDto);

            return Ok(_mapper.To<Paginated<UserForListDto>>(users));
        }

        // api/likes
        [HttpPost]
        public async Task<IActionResult> AddUserLike([FromBody] LikeForAddDto likeDto)
        {
            await _service.AddLike(base.GetUserIdFromToken(), likeDto.ReceiverId);
            return NoContent();
        }
    }
}