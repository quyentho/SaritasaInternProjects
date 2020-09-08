using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using UnrealEstate.Models;

namespace UnrealEstate.Services
{
    public interface IUserService
    {
        Task<IdentityResult> AddToAdminRole(User user);
        Task<User> GetUserByNameAsync(string username);
        Task<JwtSecurityToken> Login(AuthenticationModel model);
        Task<AuthenticationResponse> Register(AuthenticationModel model);
    }
}