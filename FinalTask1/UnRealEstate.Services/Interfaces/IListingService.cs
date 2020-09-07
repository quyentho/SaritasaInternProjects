using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.Models;

namespace UnrealEstate.Services
{
    public interface IListingService
    {
        Task AddFavoriteUser(int listingId, string userId);
        Task CreateListing(Listing listing);
        Task DisableListing(int listingId);
        Task EditListing(Listing editedListing);
        Task EnableListing(int listingId);
        Task<List<Listing>> GetActiveListingWithFilter(FilterCriteria filterCriteria);
        Task<Listing> GetListing(int listingId);
        Task<List<Listing>> GetListings();
    }
}