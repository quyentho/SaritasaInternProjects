using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;

namespace UnrealEstate.Services
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<UserViewModel> GetUserById(string userId);
        Task<List<UserViewModel>> GetUsersAsync();
    }
}