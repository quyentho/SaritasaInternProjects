using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnrealEstate.Models.Repositories;

namespace UnrealEstate.Models.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly IUserManager _userManager;

        public AuthenticationService(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public UnrealEstateUser Validate(string loginTypeCode, string email, string password)
        {
            throw new NotImplementedException();
        }

        public void Login(UnrealEstateUser user)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
