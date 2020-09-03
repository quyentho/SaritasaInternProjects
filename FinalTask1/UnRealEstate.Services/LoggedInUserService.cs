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
