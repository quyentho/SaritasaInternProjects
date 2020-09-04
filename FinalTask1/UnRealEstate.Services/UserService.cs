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

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
