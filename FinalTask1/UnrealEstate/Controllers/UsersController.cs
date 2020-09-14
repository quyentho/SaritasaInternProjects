using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;
using UnrealEstate.Services;

namespace UnrealEstate.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        public UsersController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Login, return JWT token if success authenticate user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest model)
        {
            JwtSecurityToken token = await _authenticationService.Login(model);
            if (token is null)
            {
                return BadRequest("Wrong email and password.");
            }

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
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
