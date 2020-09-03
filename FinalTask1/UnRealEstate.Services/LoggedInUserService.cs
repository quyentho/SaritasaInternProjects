using System;
using System.Collections.Generic;
using System.Text;
using UnrealEstate.Models.Repositories;
using UnRealEstate.Services;

namespace UnrealEstate.Services
{
    public abstract class LoggedInUserService : CommonUserSerive
    {
        protected LoggedInUserService(IListingRepository listingRepository, IUserManager userManager) : base(listingRepository, userManager)
        {
        }
    }
}
