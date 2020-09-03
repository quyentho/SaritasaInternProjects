using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UnrealEstate.Models.Repositories
{
    public interface IListingRepository
    {
        List<Listing> GetListings();
        
        Listing GetListingById(int listingId);

        List<Listing> GetListingsWithFilter(Expression<Func<Listing, bool>> filterConditions);
        
        void AddListing(Listing listing);

        void UpdateListing(Listing listing);
    }
}
