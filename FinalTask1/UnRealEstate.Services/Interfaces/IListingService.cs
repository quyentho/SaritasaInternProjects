using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.Models;

namespace UnrealEstate.Services
{
    public interface IListingService
    {
        Task AddFavoriteUserAsync(int listingId, string userId);
        Task CreateListingAsync(Listing listing);
        Task DisableListingAsync(User currentUser,int listingId);
        Task EditListingAsync(User currentUser, Listing listing);
        Task EnableListingAsync(int listingId);
        Task<List<Listing>> GetActiveListingWithFilterAsync(FilterCriteria filterCriteria);
        Task<Listing> GetListingAsync(int listingId);
        Task<List<Listing>> GetListingsAsync();
    }
}