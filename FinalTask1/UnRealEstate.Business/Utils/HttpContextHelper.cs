using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UnrealEstate.Business.User.Service;
using UnrealEstate.Business.User.ViewModel;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Business.Utils
{
    public static class HttpContextHelper
    {
        public static async Task<ApplicationUser> GetCurrentUserAsync(HttpContext httpContext, IUserService userService)
        {
            var userEmail =
                httpContext.User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)
                    ?.Value;

            ApplicationUser user = await userService.GetUserByEmailAsync(userEmail);

            return user;
        }

        public static async Task<UserResponse> GetCurrentUserViewModelAsync(HttpContext httpContext, IUserService userService)
        {
            var userEmail = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var currentUser = await userService.GetUserResponseByEmailAsync(userEmail);

            return currentUser;
        }
    }
}
