using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DatingApp.Core.Interfaces.Services;
using DatingApp.Core.Entities;
using DatingApp.Core.Dtos;

namespace DatingApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : CustomControllerBase
    {
        private readonly IUserRolesService _userRolesService;
        private readonly IUserPhotoService _userPhotoService;
        private readonly UserManager<User> _userManager;

        public AdminController(IUserRolesService userRolesService, IUserPhotoService userPhotoService, UserManager<User> userManager)
        {
            _userRolesService = userRolesService ?? throw new ArgumentNullException(nameof(userRolesService));
            _userPhotoService = userPhotoService ?? throw new ArgumentNullException(nameof(userPhotoService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await _userRolesService.GetUsersWithRoles();
            return Ok(userList);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDto roleEditDto)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = roleEditDto.RoleNames;

            selectedRoles = selectedRoles ?? new string[] { };
            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to add to roles.");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to remove the roles.");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-for-moderation")]
        public async Task<IActionResult> GetPhotosForModeration()
        {
            var photos = await _userPhotoService.GetPhotosForModeration();
            return Ok(photos);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("approve-photo/{photoId}")]
        public async Task<IActionResult> ApprovePhoto(int photoId)
        {
            await _userPhotoService.ApprovePhoto(photoId);
            return NoContent();
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("reject-photo/{photoId}")]
        public async Task<IActionResult> RejectPhoto(int photoId)
        {
            await _userPhotoService.RejectPhoto(photoId);
            return NoContent();
        }
    }
}