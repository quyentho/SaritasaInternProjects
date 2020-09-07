using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UnrealEstate.Models.Repositories
{
    public interface IListingRepository
    {
        Task<List<Listing>> GetListingsAsync();
        
        Task<Listing> GetListingByIdAsync(int listingId);

        Task<List<Listing>> GetListingsWithFilterAsync(Expression<Func<Listing, bool>> filterConditions);
        
        Task AddListingAsync(Listing listing);

        Task UpdateListingAsync(Listing listing);
    }
}
