using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;
using UnrealEstate.Services;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace UnrealEstate.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        
        public UsersController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// GetJwtLoginToken, return JWT token if success authenticate user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login( AuthenticationRequest model)
        {
            if (ModelState.IsValid)
            {
                SignInResult result = await _authenticationService.LoginAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            

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
        /// Register new user. Available for guest.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(AuthenticationRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            AuthenticationResponse response = await _authenticationService.Register(model);

            if (response.Status.Equals("Error"))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Send email contains token to restore password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            AuthenticationResponse authenticationResponseViewModel = await _authenticationService.SendResetPasswordEmail(model.Email);

            return Ok(authenticationResponseViewModel);
        }

        /// <summary>
        /// Verify the reset password token and set new password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            AuthenticationResponse resetPasswordResult = await _authenticationService.ResetPassword(model);

            if (resetPasswordResult.Status.Equals("Success"))
            {
                return Ok(resetPasswordResult.Message);
            }

            return BadRequest(resetPasswordResult.Message);
        }
    }
}
