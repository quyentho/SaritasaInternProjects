using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UnrealEstate.Business.Authentication.Service;
using UnrealEstate.Business.Authentication.ViewModel.Request;
using UnrealEstate.Business.Authentication.ViewModel.Response;
using UnrealEstate.Business.User.Service;
using UnrealEstate.Business.User.ViewModel;
using UnrealEstate.Business.Utils;
using UnrealEstate.Controllers.Apis;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly ApiUsersController _apiUsers;



        public UsersController(IAuthenticationService authenticationService, IUserService userService, ApiUsersController apiUsers)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _apiUsers = apiUsers;
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


        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (TempData["updateMessage"] != null)
            {
                ViewData["updateMessage"] = TempData["updateMessage"].ToString();
            }

            UserResponse currentUser = await HttpContextHelper.GetCurrentUserViewModelAsync(HttpContext, _userService);

            return View(currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                TempData["updateMessage"] = "Invalid change profile attempt";

                return RedirectToAction(nameof(Profile));
            }

            ApplicationUser currentUser = await HttpContextHelper.GetCurrentUserAsync(HttpContext, _userService);

            StatusCodeResult updateResult = (StatusCodeResult)await _apiUsers.UpdateInformation(userRequest, currentUser);

            if (updateResult.StatusCode == StatusCodes.Status204NoContent)
            {
                TempData["updateMessage"] = "Profile updated";
            }

            return RedirectToAction(nameof(Profile));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                var registerResult = (ObjectResult)await _apiUsers.Register(model);

                if (registerResult.StatusCode == StatusCodes.Status200OK)
                {
                    var loginViewModel = new LoginViewModel { Email = model.Email, Password = model.Password };

                    await _authenticationService.LoginAsync(loginViewModel);

                    return RedirectToAction("Index", "Home", loginViewModel);
                }

                AuthenticationResponse response = (AuthenticationResponse)registerResult.Value;

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ModelState.AddModelError(string.Empty, "Invalid register attempt");

            return View();
        }
    }
}