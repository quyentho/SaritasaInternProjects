using System.Collections.Generic;

namespace UnrealEstate.Models
{
    public interface IUserRepository
    {
        List<User> GetUsers();

        void AddUser(User user);

        void UpdateUser(User updatedUser);

        User GetUserById(int id);
        void RemoveUser(int id);
    }
}