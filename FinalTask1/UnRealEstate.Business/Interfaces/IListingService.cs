using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;

namespace UnrealEstate.Business.Interfaces
{
    public interface IListingService
    {
        /// <summary>
        /// Adds listing to user's favorite. If user already favorited then remove.
        /// </summary>
        /// <param name="listingId">Listing id.</param>
        /// <param name="userId">User id.</param>
        /// <returns>Bool indicates if user is favorited this listing.</returns>
        Task AddOrRemoveFavoriteAsync(int listingId, string userId);

        /// <summary>
        /// Creates new Listing.
        /// </summary>
        /// <param name="listing">New listing.</param>
        /// <returns></returns>
        Task CreateListingAsync(ListingRequest listingRequest, string userId);

        /// <summary>
        /// Disable listing if listing status is active, only available for admin user.
        /// </summary>
        /// <param name="currentUser">current logged in user.</param>
        /// <param name="listingId">listing id.</param>
        /// <returns></returns>
        Task DisableListingAsync(User currentUser, int listingId);

        /// <summary>
        /// Edits listing, only available for admin user or listing author.
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="listingRequest"></param>
        /// <param name="listingId"></param>
        /// <returns></returns>
        Task EditListingAsync(User currentUser, ListingRequest listingRequest, int listingId);

        /// <summary>
        /// Enables listing if listing status is active, only available for admin user.
        /// </summary>
        /// <param name="currentUser">current logged in user.</param>
        /// <param name="listingId">listing id.</param>
        /// <returns></returns>
        Task EnableListingAsync(User currentUser, int listingId);

        /// <summary>
        /// Gets active listing with filter.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <returns></returns>
        Task<List<ListingResponse>> GetActiveListingsWithFilterAsync(ListingFilterCriteriaRequest filterCriteria);

        /// <summary>
        /// Gets listing by id.
        /// </summary>
        /// <param name="listingId">listing id.</param>
        /// <returns></returns>
        Task<ListingResponse> GetListingAsync(int listingId);

        /// <summary>
        /// Gets all listing.
        /// </summary>
        /// <returns>list of listings.</returns>
        Task<List<ListingResponse>> GetListingsAsync();

        /// <summary>
        /// Biding on specified listing.
        /// </summary>
        /// <param name="listingId">listing id.</param>
        /// <param name="currentUser">current logged in user.</param>
        /// <param name="bidRequestViewModel">bid model.</param>
        /// <returns>A task represent biding action.</returns>
        Task MakeABid(int listingId, User currentUser, BidRequest bidRequestViewModel);

        /// <summary>
        /// Delete Photo on a listing by id.
        /// </summary>
        /// <param name="currentUser">current logged in user.</param>
        /// <param name="listingId">listing id.</param>
        /// <param name="photoId">photo id.</param>
        /// <returns></returns>
        Task DeletePhotoAsync(User currentUser, int listingId, int photoId);
    }
}