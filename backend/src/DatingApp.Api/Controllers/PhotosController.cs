using System;
using System.Threading.Tasks;
using DatingApp.Api.Helpers;
using DatingApp.Core.Dtos.Photos;
using DatingApp.Core.Interfaces.Mappers;
using DatingApp.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [Route("api/photos")]
    [ApiController]
    public class PhotosController : CustomControllerBase
    {
        private readonly IPhotosService _service;
        private readonly IUsersService _usersService;
        private readonly IClassMapper _mapper;
        private readonly ICacheService _cache;

        public PhotosController(IPhotosService service, IUsersService usersService, IClassMapper mapper, ICacheService cache)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        // api/photos/{photoId}
        [HttpGet("{photoId}", Name = nameof(GetPhoto))]
        public async Task<IActionResult> GetPhoto(int photoId)
        {
            var photo = await _service.GetPhoto(photoId);
            if (photo is null)
                return NotFound($"Could not find the photo '{photoId}'.");

            return Ok(_mapper.To<PhotoToReturnDto>(photo));
        }

        // api/photos
        [HttpPost]
        public async Task<IActionResult> AddPhoto([FromForm] PhotoForCreationDto photoDto)
        {
            var photo = await _service.UploadPhoto(base.GetUserIdFromToken(), photoDto);
            return Ok(_mapper.To<PhotoToReturnDto>(photo));
        }

        // api/photos/{photoId}/set-main
        [HttpPut("{photoId}/set-main")]
        public async Task<IActionResult> SetMainPhoto(int photoId)
        {
            await _service.SetMainPhoto(base.GetUserIdFromToken(), photoId);

            // remove from the cache by prefix
            await _cache.RemoveByPrefixAsync("users:search");

            return NoContent();
        }

        // api/photos/{photoId}
        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            await _service.DeletePhoto(base.GetUserIdFromToken(), photoId);
            return NoContent();
        }
    }
}