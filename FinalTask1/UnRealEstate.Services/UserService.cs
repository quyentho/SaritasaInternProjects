using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Services.Utils;

namespace UnrealEstate.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public UserService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<UserResponse>> GetActiveUsersWithFilterAsync(UserFilterCriteriaRequest filterCriteria)
        {
            List<User> users = await _userManager.Users
                .Include(u=>u.Comments)
                .Include(u=>u.Favorites)
                .Include(u=>u.Listings)
                .Include(u=>u.ListingNotes)
                .Include(u=>u.Bids)
                .ToListAsync();

            users = GetFilteredUsers(filterCriteria, users);

            List<UserResponse> userViewModels = MapUsersToViewModels(users);

            return userViewModels;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task UpdateUser(User currentUser, UserRequest userViewModel)
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

        public async Task<UserResponse> GetUserByIdAsync(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);

            UserResponse userViewModel = _mapper.Map<UserResponse>(user);

            return userViewModel;
        }

        private List<UserResponse> MapUsersToViewModels(List<User> users)
        {
            return _mapper.Map<List<UserResponse>>(users);
        }

        private static ExpressionStarter<User> BuildConditions(UserFilterCriteriaRequest filterCriteria)
        {
            var filterConditions = PredicateBuilder.New<User>(true);

            if (!string.IsNullOrEmpty(filterCriteria.Email))
            {
                filterConditions.And(u => u.Email.Equals(filterCriteria.Email));
            }

            if (!string.IsNullOrEmpty(filterCriteria.Name))
            {
                filterConditions.And(u => u.FirstName.Equals(filterCriteria.Name) || u.LastName.Equals(filterCriteria.Name));
            }

            return filterConditions;
        }

        private List<User> GetFilteredUsers(UserFilterCriteriaRequest filterCriteria, List<User> users)
        {
            var conditions = BuildConditions(filterCriteria);

            IQueryable<User> result = users.Where(conditions).AsQueryable();

            if (filterCriteria.Offset.HasValue || filterCriteria.Limit.HasValue)
            {
                result = result.FilterByRange((int?)filterCriteria.Offset, (int?)filterCriteria.Limit);
            }

            if (!string.IsNullOrEmpty(filterCriteria.OrderBy))
            {
                result = result.SortBy(filterCriteria.OrderBy);
            }

            return result.ToList();
        }
    }
}
