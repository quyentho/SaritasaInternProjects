﻿using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using UnrealEstate.Business.Authentication.ViewModel.Request;
using UnrealEstate.Business.Authentication.ViewModel.Response;

namespace UnrealEstate.Business.Authentication.Service
{
    public interface IAuthenticationService
    {
        /// <summary>
        ///     Authenticate user and give back jwt token if success login.
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        Task<JwtSecurityToken> GetJwtLoginToken(LoginViewModel loginViewModel);

        /// <summary>
        ///     Register new user, save to database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AuthenticationResponse> Register(RegisterRequest model);

        /// <summary>
        ///     Send reset password email that contains token.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<AuthenticationResponse> SendResetPasswordEmail(string email);

        /// <summary>
        ///     Verify reset password token and set new password if valid.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AuthenticationResponse> ResetPassword(ResetPasswordRequest model);

        /// <summary>
        ///     Login by email and password by sign in manager.
        /// </summary>
        /// <param name="loginViewModel">Request Model contains email and password.</param>
        /// <returns>Sign in result.</returns>
        Task<AuthenticationResponse> LoginAsync(LoginViewModel loginViewModel);

        /// <summary>
        ///     Logout
        /// </summary>
        /// <returns></returns>
        Task LogoutAsync();

        /// <summary>
        ///     Use external logging.
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        Task<AuthenticationResponse> ExternalLoginAsync(LoginViewModel loginViewModel);

        /// <summary>
        ///     Change Password for current logged in user.
        /// </summary>
        /// <param name="changePasswordRequest"></param>
        /// <returns></returns>
        Task<AuthenticationResponse> ChangePasswordAsync(ChangePasswordRequest changePasswordRequest);

        /// <summary>
        /// Gets authentication properties of external login after configure.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        AuthenticationProperties GetExternalAuthenticationProperties(string provider, string redirectUrl);

        /// <summary>
        /// Gets external authentication schemes.
        /// </summary>
        /// <returns></returns>
        Task<List<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
    }
}