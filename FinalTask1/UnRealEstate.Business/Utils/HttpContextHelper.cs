using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UnrealEstate.Business.User.Service;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Business.Utils
{
    public static class HttpContextHelper
    {


        public static async System.Threading.Tasks.Task<ApplicationUser> GetCurrentUserAsync(HttpContext httpContext, IUserService userService)
        {
            var userEmail =
                httpContext.User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)
                    ?.Value;

            ApplicationUser user = await userService.GetUserByEmailAsync(userEmail);

            return user;
        }
    }
}
