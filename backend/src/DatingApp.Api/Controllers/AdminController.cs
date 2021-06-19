using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DatingApp.Core.Interfaces.Services;
using DatingApp.Core.Entities;
using DatingApp.Core.Dtos;
using DatingApp.Api.Helpers;
using DatingApp.Core.Constants;

namespace DatingApp.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : CustomControllerBase
    {
        private readonly IUserRolesService _userRolesService;
        private readonly IPhotosService _photosService;
        private readonly UserManager<User> _userManager;

        public AdminController(IUserRolesService userRolesService, IPhotosService photosService, UserManager<User> userManager)
        {
            _userRolesService = userRolesService ?? throw new ArgumentNullException(nameof(userRolesService));
            _photosService = photosService ?? throw new ArgumentNullException(nameof(photosService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [Authorize(Policy = Authorizations.AdminRole)]
        [HttpGet("users-with-roles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var users = await _userRolesService.GetUsersWithRoles();
            return Ok(users);
        }

        [Authorize(Policy = Authorizations.AdminRole)]
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

        [Authorize(Policy = Authorizations.ModeratorRole)]
        [HttpGet("photos-for-moderation")]
        public async Task<IActionResult> GetPhotosForModeration()
        {
            var photos = await _photosService.GetPhotosForModeration();
            return Ok(photos);
        }

        [Authorize(Policy = Authorizations.ModeratorRole)]
        [HttpPost("approve-photo/{photoId}")]
        public async Task<IActionResult> ApprovePhoto(int photoId)
        {
            await _photosService.ApprovePhoto(photoId);
            return NoContent();
        }

        [Authorize(Policy = Authorizations.ModeratorRole)]
        [HttpPost("reject-photo/{photoId}")]
        public async Task<IActionResult> RejectPhoto(int photoId)
        {
            await _photosService.RejectPhoto(photoId);
            return NoContent();
        }
    }
}