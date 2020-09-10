using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;

namespace UnrealEstate.Services
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<UserResponseViewModel> GetUserByIdAsync(string userId);
        Task<List<UserResponseViewModel>> GetUsersAsync();
        Task UpdateUser(User currentUser, UserResponseViewModel userViewModel);
    }
}