using System.Collections.Generic;
using UnrealEstate.Models;

namespace UnrealEstate.Services
{
    public interface IUserService
    {
        void AddUser(UnrealEstateUser newUser);
        UnrealEstateUser GetUser(int id);
        List<UnrealEstateUser> GetUsers();
        void RemoveUser(int id);
        void UpdateUser(UnrealEstateUser unrealEstateUser);
    }
}