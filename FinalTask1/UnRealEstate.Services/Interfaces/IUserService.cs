using System.Collections.Generic;
using UnrealEstate.Models;

namespace UnrealEstate.Services
{
    public interface IUserService
    {
        void AddUser(User newUser);
        User GetUser(int id);
        List<User> GetUsers();
        void RemoveUser(int id);
        void UpdateUser(User unrealEstateUser);
    }
}