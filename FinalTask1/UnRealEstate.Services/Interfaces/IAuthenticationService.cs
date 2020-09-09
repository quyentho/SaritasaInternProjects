﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;

namespace UnrealEstate.Services
{
    public interface IAuthenticationService
    {
        Task<JwtSecurityToken> Login(AuthenticationViewModel model);
        Task<AuthenticationResponseViewModel> Register(AuthenticationViewModel model);
        Task<AuthenticationResponseViewModel> SendResetPasswordEmail(string email);
        Task<AuthenticationResponseViewModel> ResetPassword(ResetPasswordViewModel model);
    }
}