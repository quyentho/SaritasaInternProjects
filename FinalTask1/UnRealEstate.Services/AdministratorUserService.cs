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
