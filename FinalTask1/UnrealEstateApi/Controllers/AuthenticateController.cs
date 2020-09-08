using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Services;
using UnrealEstate.Services.EmailService;

namespace UnrealEstateApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthenticateController(IUserService userSerive)
        {
            _userService = userSerive;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationModel model)
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
        public async Task<IActionResult> Register([FromBody] AuthenticationModel model)
        {
            AuthenticationResponseModel response = await _userService.Register(model);

            if (response.Status.Equals("Error"))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] User user)
        {
            var userExists = await _userService.GetUserByEmailAsync(user.Email);

            if (userExists != null)
            {
                var result = await _userService.AddToAdminRole(userExists);
                if (result.Succeeded)
                {
                    return Ok(new AuthenticationResponseModel() { Status = "Success", Message = "User created successfully!" });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError
                        , new AuthenticationResponseModel() { Status = "Error", Message = "Add role to user failed! Please check user details and try again." });
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _userService.GetUserByEmailAsync(model.Email);
            if (user is null)
            {
                return BadRequest("Email not exists");
            }

           
            await _userService.SendResetPasswordEmail(user);
            return Ok("Please check your email to get reset link");
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            AuthenticationResponseModel resetPasswordResult = await _userService.ResetPassword(model);

            if (resetPasswordResult.Status.Equals("Success"))
            {
                return Ok(resetPasswordResult.Message);
            }

            return BadRequest(resetPasswordResult.Message);
        }
    }
}
