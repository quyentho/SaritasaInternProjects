using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace UnrealEstate.Models.Repositories
{
    public interface IListingRepository
    {
        List<Listing> GetListings();
        Listing GetListing(int listingId);

        List<Listing> GetListingsWithFilter(Expression<Func<Listing, bool>> filterConditions);
    }
}
