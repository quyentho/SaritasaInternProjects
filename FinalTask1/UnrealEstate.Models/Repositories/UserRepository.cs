using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        public bool AddUser(UnrealEstateUser user)
        {
            throw new NotImplementedException();
        }

        public UnrealEstateUser GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public List<UnrealEstateUser> GetUsers()
        {
            throw new NotImplementedException();
        }

        public bool RemoveUser(int id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(int userId, UnrealEstateUser updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}
