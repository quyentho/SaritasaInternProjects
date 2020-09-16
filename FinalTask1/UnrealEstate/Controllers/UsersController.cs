using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualBasic;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;
using UnrealEstate.Services;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// GetJwtLoginToken, return JWT token if success authenticate userUpdatedUserRequest.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AuthenticationRequest model)
        {
            if (ModelState.IsValid)
            {
                SignInResult result = await _authenticationService.LoginAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt");

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Register new userUpdatedUserRequest. Available for guest.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(AuthenticationRequest model)
        {
            if (ModelState.IsValid)
            {
                AuthenticationResponse response = await _authenticationService.Register(model);
                if (response.Status.Equals("Success"))
                {
                    await this.Login(model);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid register attempt");

            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Send email contains token to restore password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            if (ModelState.IsValid)
            {
                AuthenticationResponse authenticationResponseViewModel = await _authenticationService.SendResetPasswordEmail(model.Email);
                
                return View(authenticationResponseViewModel);
            }
            ModelState.AddModelError("", "Invalid operation");
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await GetCurrentUserViewModel();

            return View(currentUser);
        }

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

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        /// <summary>
        /// Verify the reset password token and set new password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            if (ModelState.IsValid)
            {
                AuthenticationResponse authenticationResponseViewModel = await _authenticationService.ResetPassword(model);

                return View(authenticationResponseViewModel);
            }

            ModelState.AddModelError("", "Invalid operation");

            return View();
        }

        private async Task<UserResponse> GetCurrentUserViewModel()
        {
            var user = await _userService.GetUserByIdAsync(User.Identity.GetUserId());

            return user;
        }
    }
}
