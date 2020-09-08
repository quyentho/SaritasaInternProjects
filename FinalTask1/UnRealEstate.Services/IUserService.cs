﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using UnrealEstate.Models;

namespace UnrealEstate.Services
{
    public interface IUserService
    {
        Task<IdentityResult> AddToAdminRole(User user);
        Task<User> GetUserByEmailAsync(string username);
        Task<JwtSecurityToken> Login(AuthenticationModel model);
        Task<AuthenticationResponseModel> Register(AuthenticationModel model);
        Task SendResetPasswordEmail(User user);
        Task<AuthenticationResponseModel> ResetPassword(ResetPasswordModel model);
    }
}