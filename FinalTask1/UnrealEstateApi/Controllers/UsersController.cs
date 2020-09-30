using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnrealEstate.Business;
using UnrealEstate.Business.Authentication.Interface;
using UnrealEstate.Business.Authentication.ViewModel.Request;
using UnrealEstate.Business.User.Service;
using UnrealEstate.Business.User.ViewModel;
using UnrealEstate.Business.User.ViewModel.Request;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstateApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        private readonly IUserService _userService;

        public UsersController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        /// <summary>
        ///     Get All Users. Only available for admin.
        /// </summary>
        /// <param name="userFilterCriteria"></param>
        /// <returns></returns>
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserFilterCriteriaRequest userFilterCriteria)
        {
            var userViewModels = await _userService.GetActiveUsersWithFilterAsync(userFilterCriteria);

            return Ok(userViewModels);
        }

        /// <summary>
        ///     Get User by Id, only available for admin.
        /// </summary>
        /// <param name="userId">user id.</param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> GetUserById(string userId)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var user = await _userService.GetUserResponseByIdlAsync(userId);
                return Ok(user);
            }
            catch (NotSupportedException)
            {
                return Forbid();
            }
        }

        /// <summary>
        ///     Register new user. Available for guest.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var response = await _authenticationService.Register(model);

            if (response.ResponseStatus.Equals("Error"))
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        ///     Update current logged in user's information.
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("me")]
        public async Task<IActionResult> UpdateInfomation(UserRequest userViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var currentUser = await GetCurrentUserAsync();

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

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var currentUser = await _userService.GetUserByEmailAsync(email);

            return currentUser;
        }
    }
}