using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;

namespace UnrealEstate.Services
{
    public interface IAuthenticationService 
    {
        /// <summary>
        /// Authenticate user and give back jwt token if success login.
        /// </summary>
        /// <param name="authenticationRequest"></param>
        /// <returns></returns>
        Task<JwtSecurityToken> GetJwtLoginToken(RegisterRequest authenticationRequest);

        /// <summary>
        /// Register new user, save to database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AuthenticationResponse> Register(RegisterRequest model);

        /// <summary>
        /// Send reset password email that contains token.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<AuthenticationResponse> SendResetPasswordEmail(string email);

        /// <summary>
        /// Verify reset password token and set new password if valid.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AuthenticationResponse> ResetPassword(ResetPasswordRequest model);
        
        /// <summary>
        /// Login by email and password by sign in manager.
        /// </summary>
        /// <param name="authenticationRequest">Request Model contains email and password.</param>
        /// <returns>Sign in result.</returns>
        Task<SignInResult> LoginAsync(LoginViewModel authenticationRequest);

        Task LogoutAsync();
    }
}