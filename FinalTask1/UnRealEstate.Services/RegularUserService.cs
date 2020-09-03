using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void EditListing(int id)
        {
            _listingRepository.UpdateListing(id);
        }
    }
}
