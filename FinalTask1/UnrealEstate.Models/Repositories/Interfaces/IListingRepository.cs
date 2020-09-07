using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UnrealEstate.Models.Repositories
{
    public interface IListingRepository
    {
        Task<List<Listing>> GetListings();
        
        Task<Listing> GetListingById(int listingId);

        Task<List<Listing>> GetListingsWithFilter(Expression<Func<Listing, bool>> filterConditions);
        
        Task AddListing(Listing listing);

        Task UpdateListing(Listing listing);
        Task AddFavoriteUser(int listingId, string userId);
    }
}
