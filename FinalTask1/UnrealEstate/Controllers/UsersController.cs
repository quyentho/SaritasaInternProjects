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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;

        public UsersController(IAuthenticationService authenticationService, IUserService userService,
            SignInManager<ApplicationUser> signInManager)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _signInManager = signInManager;
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
        public async Task<IActionResult> Login(string returnUrl)
        {
            _ = returnUrl ?? Url.Action("Index", "Home");
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginResult
                    = await _authenticationService.LoginAsync(model);

                if (loginResult.ResponseStatus == AuthenticationResponseStatus.Success)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return LocalRedirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, loginResult.Message);

                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");

            return View(model);
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Users", new {ReturnUrl = returnUrl});

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            var loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            var result = await _authenticationService.ExternalLoginAsync(loginViewModel);

            if (result.ResponseStatus == AuthenticationResponseStatus.Error)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View("Login", loginViewModel);
            }

            return LocalRedirect(returnUrl);
        }


        [Authorize]
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


        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = await _authenticationService.Register(model);
                if (response.ResponseStatus == AuthenticationResponseStatus.Success)
                {
                    var loginViewModel = new LoginViewModel {Email = model.Email, Password = model.Password};
                    await Login(loginViewModel);

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


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            if (ModelState.IsValid)
            {
                var authenticationResponseViewModel = await _authenticationService.SendResetPasswordEmail(model.Email);

                return View(authenticationResponseViewModel);
            }

            ModelState.AddModelError("", "Invalid operation");
            return View();
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

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            if (ModelState.IsValid)
            {
                var authenticationResponseViewModel = await _authenticationService.ResetPassword(model);

                return View(authenticationResponseViewModel);
            }

            ModelState.AddModelError(string.Empty, "Invalid operation");

            return View();
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