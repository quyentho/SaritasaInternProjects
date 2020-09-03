using UnrealEstate.Models;
using UnrealEstate.Models.Repositories;

namespace UnrealEstate.Services
{
    public class RegularUserService : LoggedInUserService
    {
        private readonly IListingRepository _listingRepository;

        public RegularUserService(IListingRepository listingRepository, IUserManager userManager) : base(listingRepository, userManager)
        {
            this._listingRepository = listingRepository;
        }

        public void CreateListing(Listing listing)
        {
            _listingRepository.AddListing(listing);
        }

        public void EditListing(Listing editedListing)
        {
            _listingRepository.UpdateListing(editedListing);
        }
    }
}
