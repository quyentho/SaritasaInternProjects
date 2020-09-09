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
        private readonly IAuthenticationService _authenticationService;
        public AuthenticateController(IAuthenticationService userSerive)
        {
            _authenticationService = userSerive;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationViewModel model)
        {
            JwtSecurityToken token = await _authenticationService.Login(model);
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
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            AuthenticationResponseViewModel authenticationResponseViewModel = await _authenticationService.SendResetPasswordEmail(model.Email);

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

            AuthenticationResponseViewModel resetPasswordResult = await _authenticationService.ResetPassword(model);

            if (resetPasswordResult.Status.Equals("Success"))
            {
                return Ok(resetPasswordResult.Message);
            }

            return BadRequest(resetPasswordResult.Message);
        }
    }
}
