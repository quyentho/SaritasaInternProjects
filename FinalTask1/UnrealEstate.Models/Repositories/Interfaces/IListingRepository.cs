using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnrealEstate.Models.Models;

namespace UnrealEstate.Models.Repositories
{
    public interface IListingRepository
    {
        Task<List<Listing>> GetListingsAsync();

        Task<Listing> GetListingByIdAsync(int listingId);


        Task AddListingAsync(Listing listing);

        Task UpdateListingAsync(Listing listing);
    }
}
