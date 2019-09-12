using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class UserPhotoController : CustomControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        private readonly Cloudinary _cloudinary;

        public UserPhotoController(IDatingRepository repo, IMapper mapper,
            IOptions<CloudinarySettings> cloudinarySettings)
        {
            _mapper = mapper;
            _repo = repo;
            _cloudinarySettings = cloudinarySettings;

            Account account = new Account(
                _cloudinarySettings.Value.CloudName,
                _cloudinarySettings.Value.ApiKey,
                _cloudinarySettings.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        // api/users/{userId}/photos/{userPhotoId}
        [HttpGet("{userPhotoId}", Name = nameof(GetUserPhoto))]
        public async Task<IActionResult> GetUserPhoto(int userPhotoId)
        {
            var userPhotoFromRepo = await _repo.GetUserPhoto(userPhotoId);

            if (userPhotoFromRepo is null)
                return NotFound($"Could not find the photo {userPhotoId}");

            // if (!base.DoesUserMatchWithToken(userPhotoFromRepo.UserId))
            //     return Unauthorized();

            var userPhotoForReturn = _mapper.Map<UserPhotoForReturnDto>(userPhotoFromRepo);

            return Ok(userPhotoForReturn);
        }

        // api/users/{userId}/photos
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId,
            [FromForm]UserPhotoForCreationDto userPhotoForCreationDto)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            var file = userPhotoForCreationDto.File;
            var uploadResult = new ImageUploadResult();

            if (!(file is null) && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }

                var photo = _mapper.Map<UserPhoto>(userPhotoForCreationDto);

                photo.Url = uploadResult.Uri.ToString();
                photo.PublicId = uploadResult.PublicId;

                if (!userFromRepo.Photos.Any(u => u.IsMain))
                    photo.IsMain = true;

                userFromRepo.Photos.Add(photo);

                if (await _repo.SaveAll())
                {
                    var userPhotoForReturnDto = _mapper.Map<UserPhotoForReturnDto>(photo);

                    return CreatedAtRoute(nameof(GetUserPhoto), new { userPhotoId = photo.Id }, userPhotoForReturnDto);
                }

                return BadRequest("Could not add the photo");
            }
            else
            {
                return BadRequest("The photo was not provided");
            }
        }

        // api/users/{userId}/photos/{userPhotoId}/setMain
        [HttpPut("{userPhotoId}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int userPhotoId)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            if (!userFromRepo.Photos.Any(p => p.Id == userPhotoId))
                return NotFound();

            var userPhotoFromRepo = await _repo.GetUserPhoto(userPhotoId);

            if (userPhotoFromRepo.IsMain)
                return BadRequest("This is already the main photo");

            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;

            userPhotoFromRepo.IsMain = true;

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Could not set user photo to main");
        }

        // api/users/{userId}/photos/{userPhotoId}
        [HttpDelete("{userPhotoId}")]
        public async Task<IActionResult> DeletePhoto(int userId, int userPhotoId)
        {
            if (!base.DoesUserMatchWithToken(userId))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            if (!userFromRepo.Photos.Any(p => p.Id == userPhotoId))
                return NotFound();

            var userPhotoFromRepo = await _repo.GetUserPhoto(userPhotoId);

            if (userPhotoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo");

            if (!string.IsNullOrWhiteSpace(userPhotoFromRepo.PublicId))
            {
                var deleteParams = new DeletionParams(userPhotoFromRepo.PublicId);
                var deletionResult = _cloudinary.Destroy(deleteParams);

                if (new string[] { "ok", "not found" }.Contains(deletionResult.Result?.ToLower()))
                    _repo.Delete(userPhotoFromRepo);
            }
            else
            {
                _repo.Delete(userPhotoFromRepo);
            }

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}