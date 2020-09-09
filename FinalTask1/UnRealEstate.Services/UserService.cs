using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels;

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

        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var userViewModels = _mapper.Map<List<UserViewModel>>(users);

            return userViewModels;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task UpdateUser(User currentUser,UserViewModel userViewModel)
        {
            var user = await _userManager.FindByEmailAsync(userViewModel.Email);

            GuardClauses.IsAuthor(currentUser.Id, user.Id);

            _mapper.Map(userViewModel, user);

            await _userManager.UpdateAsync(user);
        }

        public async Task<UserViewModel> GetUserById(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);

            UserViewModel userViewModel = _mapper.Map<UserViewModel>(user);

            return userViewModel;
        }
    }
}
