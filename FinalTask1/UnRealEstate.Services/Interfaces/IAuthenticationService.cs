﻿using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;

namespace UnrealEstate.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticate user and give back jwt token if success login.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<JwtSecurityToken> Login(AuthenticationRequestViewModel model);

        /// <summary>
        /// Register new user, save to database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AuthenticationResponseViewModel> Register(AuthenticationRequestViewModel model);

        /// <summary>
        /// Send reset password email that contains token.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<AuthenticationResponseViewModel> SendResetPasswordEmail(string email);

        /// <summary>
        /// Verify reset password token and set new password if valid.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AuthenticationResponseViewModel> ResetPassword(ResetPasswordRequestViewModel model);
    }
}