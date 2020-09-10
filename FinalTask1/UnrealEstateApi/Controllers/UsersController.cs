using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;
using UnrealEstate.Services;

namespace UnrealEstateApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        public UsersController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        [Authorize(Roles = UserRole.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserFilterCriteriaRequestViewModel userFilterCriteria)
        {
            List<UserResponseViewModel> userViewModels = await _userService.GetUsersAsync();

            return Ok(userViewModels);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> GetUserById(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                UserResponseViewModel user = await _userService.GetUserByIdAsync(userId);
                return Ok(user);
            }
            catch (NotSupportedException)
            {
                return Forbid();
            }

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] AuthenticationRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            AuthenticationResponseViewModel response = await _authenticationService.Register(model);

            if (response.Status.Equals("Error"))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("me")]
        public async Task<IActionResult> UpdateInfomation(UserRequestViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User currentUser = await GetCurrentUserAsync();

            try
            {
                await _userService.UpdateUser(currentUser, userViewModel);
            }
            catch (NotSupportedException)
            {
                return Forbid();
            }

            return NoContent();
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            User currentUser = await _userService.GetUserByEmailAsync(email);

            return currentUser;
        }

    }
}
