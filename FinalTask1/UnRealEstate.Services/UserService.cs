using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Services.EmailService;

namespace UnrealEstate.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        public async Task<IdentityResult> AddToAdminRole(User user)
        {
            if (await _roleManager.RoleExistsAsync(UserRole.Admin))
            {
                IdentityResult result =  await _userManager.AddToRoleAsync(user, UserRole.Admin);
                if (result.Succeeded)
                {
                    return result;
                }
            }

            return IdentityResult.Failed();
        }

        public async Task<User> GetUserByEmailAsync(string email) 
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<JwtSecurityToken> Login(AuthenticationModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return token;
            }

            return null;
        }

        public async Task<AuthenticationResponse> Register(AuthenticationModel model)
        {

            var userExists = await _userManager.FindByNameAsync(model.Email);
            if (userExists != null)
            {
                return new AuthenticationResponse() { Status = "Error", Message = "User creation failed! Please check user details and try again." };
            }

            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return new AuthenticationResponse() { Status = "Error", Message = "User creation failed! Please check user details and try again." };
            }

            return new AuthenticationResponse() { Status = "Success", Message = "User created successfully!" };
        }

        public async Task<AuthenticationResponse> ResetPassword(ResetPasswordModel model)
        {

            if (!model.NewPassword.Equals(model.ConfirmPassword))
            {
                return new AuthenticationResponse() { Status = "Fail", Message = "Password and confirm password does not match." };
            }
            
            var user = await _userManager.FindByEmailAsync(model.Email);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                return new AuthenticationResponse() { Status = "Fail", Message = "Wrong token" };
            }

            return new AuthenticationResponse() { Status = "Success", Message = "Password reset successfully" };
        }

        public async Task SendResetPasswordEmail(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            Message resetPasswordMessage = new Message(new string[] { user.Email }
            , "Reset password token", $"Your token is: {token}\nPlease use this end point to reset your password 'api/auth/reset-password'");

            await _emailSender.SendEmailAsync(resetPasswordMessage);
        }
    }
}
