using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UnrealEstate.Business.Authentication.Interface;
using UnrealEstate.Business.Authentication.ViewModel.Request;
using UnrealEstate.Business.Authentication.ViewModel.Response;
using UnrealEstate.Business.User.Service;
using UnrealEstate.Business.User.ViewModel;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public UsersController(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest model)
        {
            if (ModelState.IsValid)
            {
                model.Email = User.FindFirstValue(ClaimTypes.Email);

                var result = await _authenticationService.ChangePasswordAsync(model);

                if (result.ResponseStatus == AuthenticationResponseStatus.Fail)
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                }

                return View("Profile");
            }

            ModelState.AddModelError(string.Empty, "Invalid attempt to change password");

            return View("Profile");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await GetCurrentUserViewModel();

            return View(currentUser);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(UserRequest userUpdatedUserRequest)
        {
            if (ModelState.IsValid)
            {
                // BUG: Email null 
                var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                var user = await _userService.GetUserByEmailAsync(email);

                await _userService.UpdateUser(user, userUpdatedUserRequest);
            }

            var userResponse = await GetCurrentUserViewModel();

            return View(userResponse);
        }

        private async Task<UserResponse> GetCurrentUserViewModel()
        {
            var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var currentUser = await _userService.GetUserByEmailAsync(userEmail);

            var user = await _userService.GetUserResponseByEmailAsync(currentUser.Id);

            return user;
        }
    }
}