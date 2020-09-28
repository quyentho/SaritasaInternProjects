using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UnrealEstate.Infrastructure.Models;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Services.User.Interface;
using UnrealEstate.Services.Utils;

namespace UnrealEstate.Services.User
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserResponse> GetUserResponseByIdlAsync(string userEmail)
        {
            var user = await _userManager.FindByIdAsync(userEmail);

            var userViewModel = _mapper.Map<UserResponse>(user);

            return userViewModel;
        }

        public async Task<List<UserResponse>> GetActiveUsersWithFilterAsync(UserFilterCriteriaRequest filterCriteria)
        {
            var users = await _userManager.Users
                .Include(u => u.Comments)
                .Include(u => u.Favorites)
                .Include(u => u.Listings)
                .Include(u => u.ListingNotes)
                .Include(u => u.Bids)
                .ToListAsync();

            users = GetFilteredUsers(filterCriteria, users);

            var userViewModels = MapUsersToViewModels(users);

            return userViewModels;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task UpdateUser(ApplicationUser currentUser, UserRequest userViewModel)
        {
            var user = await _userManager.FindByEmailAsync(currentUser.Email);

            _mapper.Map(userViewModel, user);

            await _userManager.UpdateAsync(user);
        }

        public async Task SetUserStatusAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            user.IsActive = !user.IsActive;

            await _userManager.UpdateAsync(user);
        }

        public async Task<UserResponse> GetUserResponseByEmailAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            var userViewModel = _mapper.Map<UserResponse>(user);

            return userViewModel;
        }

        private List<UserResponse> MapUsersToViewModels(List<ApplicationUser> users)
        {
            return _mapper.Map<List<UserResponse>>(users);
        }

        private static ExpressionStarter<ApplicationUser> BuildConditions(UserFilterCriteriaRequest filterCriteria)
        {
            var filterConditions = PredicateBuilder.New<ApplicationUser>(true);

            if (!string.IsNullOrEmpty(filterCriteria.Email))
                filterConditions.And(u => u.Email.Equals(filterCriteria.Email));

            if (!string.IsNullOrEmpty(filterCriteria.Name))
                filterConditions.And(u =>
                    u.FirstName.Equals(filterCriteria.Name) || u.LastName.Equals(filterCriteria.Name));

            return filterConditions;
        }

        private List<ApplicationUser> GetFilteredUsers(UserFilterCriteriaRequest filterCriteria,
            List<ApplicationUser> users)
        {
            var conditions = BuildConditions(filterCriteria);

            var result = users.Where(conditions).AsQueryable();

            if (filterCriteria.Offset.HasValue || filterCriteria.Limit.HasValue)
                result = result.FilterByRange((int?) filterCriteria.Offset, (int?) filterCriteria.Limit);

            if (!string.IsNullOrEmpty(filterCriteria.OrderBy)) result = result.SortBy(filterCriteria.OrderBy);

            return result.ToList();
        }
    }
}