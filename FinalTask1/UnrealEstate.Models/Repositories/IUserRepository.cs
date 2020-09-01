using System.Collections.Generic;

namespace UnrealEstate.Models
{
    public interface IUserRepository
    {
        List<UnrealEstateUser> GetUsers();

        bool AddUser(UnrealEstateUser user);

        bool UpdateUser(int userId, UnrealEstateUser updatedUser);

        UnrealEstateUser GetUser(int id);
        bool RemoveUser(int id);
    }
}