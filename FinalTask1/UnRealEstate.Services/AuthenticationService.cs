using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;
using UnrealEstate.Services.EmailService;

namespace UnrealEstate.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderService _emailSender;
        private readonly SignInManager<User> _signInManager;
        public AuthenticationService(
            UserManager<User> userManager,
            IConfiguration configuration,
            IEmailSenderService emailSender,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Sign out.
        /// </summary>
        /// <returns></returns>
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        /// <inheritdoc />
        public async Task<SignInResult> LoginAsync(LoginViewModel authenticationRequest)
        {
            var user = await _userManager.FindByEmailAsync(authenticationRequest.Email);

            if (user != null)
            {
                SignInResult result = await
                    _signInManager.PasswordSignInAsync(user, authenticationRequest.Password, false, false);
                return result;
            }

            return SignInResult.Failed;
        }

        /// <inheritdoc/>
        public async Task<JwtSecurityToken> GetJwtLoginToken(RegisterRequest authenticationRequest)
        {
            var user = await _userManager.FindByEmailAsync(authenticationRequest.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, authenticationRequest.Password))
            {
                JwtSecurityToken token = await CreateToken(user);

                return token;
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> Register(RegisterRequest model)
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

            if (result.Succeeded)
            {
                var claim = new Claim(ClaimTypes.Email, user.Email);
                await _userManager.AddClaimAsync(user, claim);
                await _userManager.AddToRoleAsync(user, UserRole.User);

                return new AuthenticationResponse() { Status = "Success", Message = "User created successfully!" };
            }

            return new AuthenticationResponse() { Status = "Error", Message = "User creation failed! Please check user details and try again." };
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> ResetPassword(ResetPasswordRequest model)
        {

            bool isPasswordConfirmNotMatched = !model.NewPassword.Equals(model.ConfirmPassword);

            if (isPasswordConfirmNotMatched)
            {
                return new AuthenticationResponse() { Status = "Fail", Message = "Password and confirm password does not match." };
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                return new AuthenticationResponse() { Status = "Success", Message = "Password reset successfully" };
            }

            return new AuthenticationResponse() { Status = "Fail", Message = result.Errors.Select(e => e.Description).Aggregate((message, next) => message + next) };

        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> SendResetPasswordEmail(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return new AuthenticationResponse() { Status = "Fail", Message = "Email is not correct." };
            }

            await SendEmail(user);

            return new AuthenticationResponse() { Status = "Success", Message = "Please check your email to get reset link" };
        }

        private async Task SendEmail(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var bodyBuilder = new BodyBuilder
            {
                TextBody =
                    $"Your token is: {token}\nPlease use this end point to reset your password 'api/auth/reset-password' ",
                HtmlBody = "<a href='http://unrealestate.com/users/forgotpassword'>this link</a>"
            };

            Message resetPasswordMessage = new Message(new string[] { user.Email }
            , "Reset password token", bodyBuilder.ToMessageBody().ToString());

            await _emailSender.SendEmailAsync(resetPasswordMessage);
        }

        private async Task<JwtSecurityToken> CreateToken(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, user.Email),
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
    }
}
