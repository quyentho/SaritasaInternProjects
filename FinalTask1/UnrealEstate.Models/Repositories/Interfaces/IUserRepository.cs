using System.Collections.Generic;

namespace UnrealEstate.Models
{
    public interface IUserRepository
    {
        List<UnrealEstateUser> GetUsers();

        void AddUser(UnrealEstateUser user);

        void UpdateUser(UnrealEstateUser updatedUser);

        UnrealEstateUser GetUserById(int id);
        void RemoveUser(int id);
    }
}