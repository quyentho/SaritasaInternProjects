using System.Collections.Generic;
using UnrealEstate.Models;
using UnrealEstate.Models.Repositories;

namespace UnrealEstate.Services
{
    public class AdministratorUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IListingRepository _listingRepository;

        public AdministratorUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public AdministratorUserService(IUserRepository userRepository, IListingRepository listingRepository)
        {
            _userRepository = userRepository;
            _listingRepository = listingRepository;
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

        public void DisableListing(int listingId)
        {
            _listingRepository.Disable(listingId);
        }

    }
}
