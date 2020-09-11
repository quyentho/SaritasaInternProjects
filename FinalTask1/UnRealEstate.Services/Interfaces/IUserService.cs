using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;

namespace UnrealEstate.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Gets user by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserResponse> GetUserByIdAsync(string userId);

        /// <summary>
        /// Get users after filtered.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <returns></returns>
        Task<List<UserResponse>> GetUsersWithFilterAsync(UserFilterCriteriaRequest filterCriteria);

        /// <summary>
        /// Update current user's info.
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        Task UpdateUser(User currentUser, UserRequest userViewModel);
    }
}