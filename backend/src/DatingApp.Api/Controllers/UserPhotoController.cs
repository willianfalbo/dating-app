using System;
using System.Threading.Tasks;
using DatingApp.Core.Dtos;
using DatingApp.Core.Interfaces;
using DatingApp.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    // [Authorize] // once we are using AspNet Core Identity, we dont need this line anymore
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class UserPhotoController : CustomControllerBase
    {
        private readonly IUserPhotoService _service;
        private readonly IUserService _userService;
        private readonly IClassMapper _mapper;

        public UserPhotoController(IUserPhotoService service, IUserService userService, IClassMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // api/users/{userId}/photos/{userPhotoId}
        [HttpGet("{userPhotoId}", Name = nameof(GetUserPhoto))]
        public async Task<IActionResult> GetUserPhoto(int userPhotoId)
        {
            var userPhotoFromRepo = await _service.GetUserPhoto(userPhotoId);

            if (userPhotoFromRepo is null)
                return NotFound($"Could not find the photo {userPhotoId}");

            // if (!base.DoesUserMatchWithToken(userPhotoFromRepo.UserId))
            //     return Unauthorized();

            var userPhotoForReturn = _mapper.To<UserPhotoForReturnDto>(userPhotoFromRepo);

            return Ok(userPhotoForReturn);
        }

        // api/users/{userId}/photos
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm] UserPhotoForCreationDto userPhotoForCreationDto)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            var userPhoto = await _service.UploadUserPhoto(userId, userPhotoForCreationDto);

            return Ok(_mapper.To<UserPhotoForReturnDto>(userPhoto));
        }

        // api/users/{userId}/photos/{userPhotoId}/setMain
        [HttpPut("{userPhotoId}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int userPhotoId)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            await _service.SetMainPhoto(userId, userPhotoId);

            return NoContent();
        }

        // api/users/{userId}/photos/{userPhotoId}
        [HttpDelete("{userPhotoId}")]
        public async Task<IActionResult> DeletePhoto(int userId, int userPhotoId)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            await _service.DeletePhoto(userId, userPhotoId);

            return NoContent();
        }
    }
}