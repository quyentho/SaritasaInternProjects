using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;

namespace UnrealEstate.Services
{
    public interface IAuthenticationService
    {
        Task<JwtSecurityToken> Login(AuthenticationRequest model);
        Task<AuthenticationResponse> Register(AuthenticationRequest model);
        Task<AuthenticationResponse> SendResetPasswordEmail(string email);
        Task<AuthenticationResponse> ResetPassword(ResetPasswordRequest model);
    }
}