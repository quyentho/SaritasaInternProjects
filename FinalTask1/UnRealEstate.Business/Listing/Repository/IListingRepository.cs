using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnrealEstate.Business.Listing.Repository
{
    public interface IListingRepository
    {
        Task<List<Infrastructure.Models.Listing>> GetListingsAsync();

        Task<Infrastructure.Models.Listing> GetListingByIdAsync(int listingId);

        Task AddListingAsync(Infrastructure.Models.Listing listing);

        Task UpdateListingAsync(Infrastructure.Models.Listing listing);
    }
}