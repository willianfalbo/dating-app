using System;
using System.Threading.Tasks;
using DatingApp.Core.Dtos.UserPhotos;
using DatingApp.Core.Interfaces;
using DatingApp.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [Route("api/photos")]
    [ApiController]
    public class PhotosController : CustomControllerBase
    {
        private readonly IUserPhotoService _service;
        private readonly IUserService _userService;
        private readonly IClassMapper _mapper;

        public PhotosController(IUserPhotoService service, IUserService userService, IClassMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // api/photos/{photoId}
        [HttpGet("{photoId}", Name = nameof(GetPhoto))]
        public async Task<IActionResult> GetPhoto(int photoId)
        {
            var userPhotoFromRepo = await _service.GetUserPhoto(photoId);
            if (userPhotoFromRepo is null)
                return NotFound($"Could not find the photo '{photoId}'.");

            var userPhotoForReturn = _mapper.To<UserPhotoToReturnDto>(userPhotoFromRepo);
            return Ok(userPhotoForReturn);
        }

        // api/photos
        [HttpPost]
        public async Task<IActionResult> AddPhoto([FromForm] UserPhotoForCreationDto userPhotoForCreationDto)
        {
            var userPhoto = await _service.UploadUserPhoto(base.GetUserIdFromToken(), userPhotoForCreationDto);
            return Ok(_mapper.To<UserPhotoToReturnDto>(userPhoto));
        }

        // api/photos/{photoId}/set-main
        [HttpPut("{photoId}/set-main")]
        public async Task<IActionResult> SetMainPhoto(int photoId)
        {
            await _service.SetMainPhoto(base.GetUserIdFromToken(), photoId);
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