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
using UnrealEstate.Business.Authentication.ViewModel.Response;
using UnrealEstate.Business.User.Service;
using UnrealEstate.Business.User.ViewModel;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Controllers.Apis
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUsersController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        private readonly IUserService _userService;

        public ApiUsersController(IUserService userService, IAuthenticationService authenticationService)
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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            AuthenticationResponse response = await _authenticationService.Register(registerRequest);

            if (response.ResponseStatus == AuthenticationResponseStatus.Error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        /// <summary>
        ///     Update current logged in user's information.
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("me")]
        public async Task<IActionResult> UpdateInformation(UserRequest userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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