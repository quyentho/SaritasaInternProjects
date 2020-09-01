using System;
using System.Collections.Generic;
using UnrealEstate.Models;

namespace UnrealEstate.Services
{
    public class AdministratorUserService
    {
        private readonly IUserRepository _userRepository;

        public AdministratorUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<UnrealEstateUser> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public UnrealEstateUser GetUser(int id)
        {
            return _userRepository.GetUser(id);
        }

        public bool AddUser(UnrealEstateUser newUser)
        {
            return _userRepository.AddUser(newUser);
        }

        public bool UpdateUser(int id, UnrealEstateUser unrealEstateUser)
        {
            return _userRepository.UpdateUser(id, unrealEstateUser);
        }

        public bool RemoveUser(int id)
        {
            return _userRepository.RemoveUser(id);
        }
    }
}
