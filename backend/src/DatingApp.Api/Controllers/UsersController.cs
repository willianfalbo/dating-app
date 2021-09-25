using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Api.Helpers;
using DatingApp.Core.Dtos.Users;
using DatingApp.Core.Interfaces.Mappers;
using DatingApp.Core.Interfaces.Services;
using DatingApp.Core.Models;
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
        private readonly ICacheService _cache;

        public UsersController(IUsersService service, ILikesService likesService, IClassMapper mapper, ICacheService cache)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _likesService = likesService ?? throw new ArgumentNullException(nameof(likesService));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        // api/users
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserForFilterDto filter)
        {
            // return from the cache if exists
            var cacheKey = $"users:search:sorted_by:{filter.OrderBy}:gender:{filter.Gender}:age_between_{filter.MinAge}_{filter.MaxAge}:page_size:{filter.Limit}:page:{filter.Page}";
            var cachedResult = await _cache.GetAsync<Paginated<UserForListDto>>(cacheKey);
            if (cachedResult != null && cachedResult.Items.Any())
                return Ok(cachedResult);

            var users = await _service.GetUsers(filter);

            var result = _mapper.To<Paginated<UserForListDto>>(users);

            // add to the cache
            await _cache.SetAsync(cacheKey, result);

            // we remove the current because it doesn't make sense see ourselfs in the response
            result.Items = result.Items.Where(u => u.Id != base.GetUserIdFromToken());

            return Ok(result);
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

            // remove from the cache by prefix
            await _cache.RemoveByPrefixAsync("users:search");

            return Ok(_mapper.To<UserForDetailedDto>(user));
        }
    }
}
