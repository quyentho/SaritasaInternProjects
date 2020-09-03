using System.Collections.Generic;

namespace UnrealEstate.Models
{
    public interface IUserRepository
    {
        List<UnrealEstateUser> GetUsers();

        void AddUser(UnrealEstateUser user);

        void UpdateUser(int userId, UnrealEstateUser updatedUser);

        UnrealEstateUser GetUser(int id);
        void RemoveUser(int id);
    }
}