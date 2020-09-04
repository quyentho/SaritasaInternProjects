using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.Repositories;

namespace UnrealEstate.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserManager _userManager;

        public UserService(IUserRepository userRepository, IUserManager userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public UnrealEstateUser Validate(string loginTypeCode, string email, string password) 
        {
            throw new NotImplementedException();
        }

        public void Login(UnrealEstateUser user)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public List<UnrealEstateUser> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public UnrealEstateUser GetUser(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public void AddUser(UnrealEstateUser newUser)
        {
            _userRepository.AddUser(newUser);
        }

        public void UpdateUser(UnrealEstateUser unrealEstateUser)
        {
            _userRepository.UpdateUser(unrealEstateUser);
        }

        public void RemoveUser(int id)
        {
            _userRepository.RemoveUser(id);
        }
    }
}
