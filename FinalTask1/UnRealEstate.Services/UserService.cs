﻿using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task<List<UserResponseViewModel>> GetUsersWithFilterAsync(UserFilterCriteriaRequestViewModel filterCriteria)
        {

            List<User> users = await _userManager.Users.ToListAsync();

            users = GetFilteredUsers(filterCriteria, users);

            List<UserResponseViewModel> userViewModels = MapUsersToViewModels(users);

            return userViewModels;
        }

        private List<UserResponseViewModel> MapUsersToViewModels(List<User> users)
        {
            return _mapper.Map<List<UserResponseViewModel>>(users);
        }

        private static ExpressionStarter<User> BuildConditions(UserFilterCriteriaRequestViewModel filterCriteria)
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

        private List<User> GetFilteredUsers(UserFilterCriteriaRequestViewModel filterCriteria, List<User> users)
        {
            var conditions = BuildConditions(filterCriteria);

            IQueryable<User> result = users.Where(conditions).AsQueryable();

            result = result.FilterByRange((int?)filterCriteria.Offset, (int?)filterCriteria.Limit);

            result = result.SortBy(filterCriteria.OrderBy);

            return result.ToList();
        }

        public async Task<List<UserResponseViewModel>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var userViewModels = _mapper.Map<List<UserResponseViewModel>>(users);

            return userViewModels;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task UpdateUser(User currentUser, UserResponseViewModel userViewModel)
        {
            var user = await _userManager.FindByEmailAsync(userViewModel.Email);

            GuardClauses.IsAuthor(currentUser.Id, user.Id);

            _mapper.Map(userViewModel, user);

            await _userManager.UpdateAsync(user);
        }

        public async Task<UserResponseViewModel> GetUserByIdAsync(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);

            UserResponseViewModel userViewModel = _mapper.Map<UserResponseViewModel>(user);

            return userViewModel;
        }
    }
}
