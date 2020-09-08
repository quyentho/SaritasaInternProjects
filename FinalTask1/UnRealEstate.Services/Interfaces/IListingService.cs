using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.Models;

namespace UnrealEstate.Services
{
    public interface IListingService
    {
        /// <summary>
        /// Adds listing to user's favorite. If user already favorited then remove.
        /// </summary>
        /// <param name="listingId">Listing id.</param>
        /// <param name="userId">User id.</param>
        /// <returns>Bool indicates if user is favorited this listing.</returns>
        Task<bool> AddOrRemoveFavoriteUserAsync(int listingId, string userId);

        /// <summary>
        /// Creates new Listing.
        /// </summary>
        /// <param name="listing">New listing.</param>
        /// <returns></returns>
        Task CreateListingAsync(Listing listing);

        /// <summary>
        /// Disable listing if listing status is active, only available for admin user.
        /// </summary>
        /// <param name="currentUser">current logged in user.</param>
        /// <param name="listingId">listing id.</param>
        /// <returns></returns>
        Task DisableListingAsync(User currentUser,int listingId);

        /// <summary>
        /// Edits listing, only available for admin user or listing author.
        /// </summary>
        /// <param name="currentUser">current logged in user.</param>
        /// <param name="listing">listing updated.</param>
        /// <returns></returns>
        Task EditListingAsync(User currentUser, Listing listing);

        /// <summary>
        /// Enables listing if listing status is active, only available for admin user.
        /// </summary>
        /// <param name="currentUser">current logged in user.</param>
        /// <param name="listingId">listing id.</param>
        /// <returns></returns>
        Task EnableListingAsync(int listingId);

        /// <summary>
        /// Gets active listing with filter.
        /// </summary>
        /// <param name="filterCriteria">filter criteria.</param>
        /// <returns>Listing after filtered.</returns>
        Task<List<Listing>> GetActiveListingWithFilterAsync(FilterCriteriaModel filterCriteria);
        
        /// <summary>
        /// Gets listing by id.
        /// </summary>
        /// <param name="listingId">listing id.</param>
        /// <returns></returns>
        Task<Listing> GetListingAsync(int listingId);

        /// <summary>
        /// Gets all listing.
        /// </summary>
        /// <returns>list of listings.</returns>
        Task<List<Listing>> GetListingsAsync();
    }
}