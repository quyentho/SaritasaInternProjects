using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Models.Models;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;

namespace UnrealEstate.Services.User.Interface
{
    public interface IUserService
    {
        /// <summary>
        /// Gets user by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<ApplicationUser> GetUserByEmailAsync(string email);

        /// <summary>
        /// Get user response model by email.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        Task<UserResponse> GetUserResponseByEmailAsync(string userEmail);

        /// <summary>
        /// Get user response model by id.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        Task<UserResponse> GetUserResponseByIdlAsync(string userEmail);

        /// <summary>
        /// Get users after filtered.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <returns></returns>
        Task<List<UserResponse>> GetActiveUsersWithFilterAsync(UserFilterCriteriaRequest filterCriteria);

        /// <summary>
        /// Update current user's info.
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        Task UpdateUser(ApplicationUser currentUser, UserRequest userViewModel);

        Task SetUserStatusAsync(string email);
    }
}