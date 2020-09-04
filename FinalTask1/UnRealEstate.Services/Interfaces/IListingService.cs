using System.Collections.Generic;
using UnrealEstate.Models;
using UnrealEstate.Models.Models;

namespace UnrealEstate.Services
{
    public interface IListingService
    {
        void CreateListing(Listing listing);
        void DisableListing(int listingId);
        void EditListing(Listing editedListing);
        List<Listing> GetActiveListingWithFilter(FilterCriteria filterCriteria);
        Listing GetListing(int listingId);
        List<Listing> GetListings();
    }
}