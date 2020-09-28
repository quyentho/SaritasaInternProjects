using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using UnrealEstate.Infrastructure.Models;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;
using UnrealEstate.Services.Authentication.Interface;
using UnrealEstate.Services.EmailService;

namespace UnrealEstate.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderService _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IEmailSenderService emailSender,
            SignInManager<ApplicationUser> signInManager)
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

        public async Task<AuthenticationResponse> ChangePasswordAsync(ChangePasswordRequest changePasswordRequest)
        {
            ApplicationUser loggedInUser = await _userManager.FindByEmailAsync(changePasswordRequest.Email);
            IdentityResult result = await _userManager.ChangePasswordAsync(loggedInUser, changePasswordRequest.CurrentPassword,
                changePasswordRequest.NewPassword);

            if (result == IdentityResult.Success)
            {
                return new AuthenticationResponse()
                {
                    ResponseStatus = AuthenticationResponseStatus.Success,
                };
            }

            return new AuthenticationResponse()
            {
                ResponseStatus = AuthenticationResponseStatus.Fail,
                Message = "Current password is incorrect !"
            };
        }

        public async Task<AuthenticationResponse> ExternalLoginAsync(LoginViewModel loginViewModel)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                return new AuthenticationResponse()
                {
                    ResponseStatus = AuthenticationResponseStatus.Error,
                    Message = "Error loading external login information"
                };
            }

            var signInResult =
                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
            if (signInResult.Succeeded)
            {
                return new AuthenticationResponse()
                {
                    ResponseStatus = AuthenticationResponseStatus.Success,
                    Message = "Login successfully"
                };
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if (email == null)
            {
                return new AuthenticationResponse()
                {
                    ResponseStatus = AuthenticationResponseStatus.Error,
                    Message = $"Email Claim not received from {info.LoginProvider}"
                };
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new ApplicationUser()
                {
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                };

                await _userManager.CreateAsync(user);

                Claim claim = new Claim(ClaimTypes.Email, email);
                await _userManager.AddClaimAsync(user, claim);
            }

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: false);

            return new AuthenticationResponse()
            {
                ResponseStatus = AuthenticationResponseStatus.Success,
                Message = "Login successfully"
            };
        }

        /// <inheritdoc />
        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user is null)
            {
                return new AuthenticationResponse()
                {
                    ResponseStatus = AuthenticationResponseStatus.Fail,
                    Message = "Wrong Email"
                };
            }

            SignInResult result = await
                      _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

            if (result == SignInResult.Failed)
            {
                return new AuthenticationResponse()
                {
                    ResponseStatus = AuthenticationResponseStatus.Fail,
                    Message = "Wrong password"
                };
            }

            return new AuthenticationResponse()
            {
                ResponseStatus = AuthenticationResponseStatus.Success
            };
        }

        /// <inheritdoc/>
        public async Task<JwtSecurityToken> GetJwtLoginToken(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginViewModel.Password))
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
                return new AuthenticationResponse() { ResponseStatus = AuthenticationResponseStatus.Error, Message = "User creation failed! Please check user details and try again." };
            }

            ApplicationUser user = new ApplicationUser()
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

                return new AuthenticationResponse() { ResponseStatus = AuthenticationResponseStatus.Success, Message = "User created successfully!" };
            }

            return new AuthenticationResponse() { ResponseStatus = AuthenticationResponseStatus.Error, Message = "User creation failed! Please check user details and try again." };
        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> ResetPassword(ResetPasswordRequest model)
        {

            bool isPasswordConfirmNotMatched = !model.NewPassword.Equals(model.ConfirmPassword);

            if (isPasswordConfirmNotMatched)
            {
                return new AuthenticationResponse() { ResponseStatus = AuthenticationResponseStatus.Fail, Message = "Password and confirm password does not match." };
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                return new AuthenticationResponse() { ResponseStatus = AuthenticationResponseStatus.Success, Message = "Password reset successfully" };
            }

            return new AuthenticationResponse() { ResponseStatus = AuthenticationResponseStatus.Fail, Message = result.Errors.Select(e => e.Description).Aggregate((message, next) => message + next) };

        }

        /// <inheritdoc/>
        public async Task<AuthenticationResponse> SendResetPasswordEmail(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return new AuthenticationResponse() { ResponseStatus = AuthenticationResponseStatus.Fail, Message = "Email is not correct." };
            }

            await SendEmail(user);

            return new AuthenticationResponse() { ResponseStatus = AuthenticationResponseStatus.Success, Message = "Please check your email to get reset link" };
        }

        private async Task SendEmail(ApplicationUser user)
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

        private async Task<JwtSecurityToken> CreateToken(ApplicationUser user)
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

