using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Services;

namespace UnrealEstateApi.Controllers
{
    [Route("api/[controller]")]
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
            AuthenticationResponse response = await _userService.Register(model);

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
            var userExists = await _userService.GetUserByNameAsync(user.UserName);

            if (userExists != null)
            {
                var result = await _userService.AddToAdminRole(userExists);
                if (result.Succeeded)
                {
                    return Ok(new AuthenticationResponse() { Status = "Success", Message = "User created successfully!" });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError
                        , new AuthenticationResponse() { Status = "Error", Message = "Add role to user failed! Please check user details and try again." });
        }
    }
}
