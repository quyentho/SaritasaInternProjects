using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Services;
using UnrealEstate.Services.EmailService;

namespace UnrealEstateApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticationService _userService;
        public AuthenticateController(IAuthenticationService userSerive)
        {
            _userService = userSerive;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationViewModel model)
        {
            JwtSecurityToken token = await _userService.Login(model);
            if (token is null)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] AuthenticationViewModel model)
        {
            AuthenticationResponseViewModel response = await _userService.Register(model);

            if (response.Status.Equals("Error"))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            AuthenticationResponseViewModel authenticationResponseViewModel = await _userService.SendResetPasswordEmail(model.Email);

            return Ok(authenticationResponseViewModel);
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            AuthenticationResponseViewModel resetPasswordResult = await _userService.ResetPassword(model);

            if (resetPasswordResult.Status.Equals("Success"))
            {
                return Ok(resetPasswordResult.Message);
            }

            return BadRequest(resetPasswordResult.Message);
        }
    }
}
