using LinqKit;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.Models;
using UnrealEstate.Models.Repositories;

namespace UnrealEstate.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingRepository _listingRepository;
        private readonly UserManager<User> _userManager;

        public ListingService(IListingRepository listingRepository, UserManager<User> userManager)
        {
            _listingRepository = listingRepository;
            _userManager = userManager;
            
        }

        public async Task AddFavoriteUserAsync(int listingId, string userId)
        {
            var listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);

            GuardClauses.IsNotNull(userId, "user id");

            GuardClauses.HasValue(listingFromDb, "listing id");

            await _listingRepository.AddFavoriteUserAsync(listingId, userId);
        }

        public async Task EnableListingAsync(int listingId)
        {
            var listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);

            GuardClauses.HasValue(listingFromDb, "listing id");

            GuardClauses.IsValidStatus(listingFromDb.StatusId, (int)Status.Disable);

            listingFromDb.StatusId = (int)Status.Active; // respresents Actived status.

            await _listingRepository.UpdateListingAsync(listingFromDb);
        }

        // TODO: Add comments
        public async Task DisableListingAsync(User currentUser,int listingId)
        {
            IList<string> userRole = await GetUserRole(currentUser);
            
            var listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);

            GuardClauses.IsAdmin(userRole.First());

            GuardClauses.HasValue(listingFromDb, "listing id");

            GuardClauses.IsValidStatus(listingFromDb.StatusId, (int)Status.Active);

            listingFromDb.StatusId = (int)Status.Disable; // respresents canceled status.

            await _listingRepository.UpdateListingAsync(listingFromDb);
        }

        /// <summary>
        /// Gets list of listings.
        /// </summary>
        /// <returns>List of listing</returns>
        public Task<List<Listing>> GetListingsAsync() => _listingRepository.GetListingsAsync();

        public Task<Listing> GetListingAsync(int listingId)
        {
            return _listingRepository.GetListingByIdAsync(listingId);
        }

        public Task<List<Listing>> GetActiveListingWithFilterAsync(FilterCriteria filterCriteria)
        {
            ExpressionStarter<Listing> filterConditions = BuildConditions(filterCriteria);

            return _listingRepository.GetListingsWithFilterAsync(filterConditions);
        }

        public async Task CreateListingAsync(Listing listing)
        {
            await _listingRepository.AddListingAsync(listing);
        }

        public async Task EditListingAsync(User currentUser,Listing listing)
        {
            var listingFromDb = await _listingRepository.GetListingByIdAsync(listing.Id);
            
            IList<string> userRole = await GetUserRole(currentUser);

            GuardClauses.HasValue(listingFromDb, "listing");

            GuardClauses.IsAuthorOrAdmin(currentUser.Id, listingFromDb.UserId, userRole.First());

            await _listingRepository.UpdateListingAsync(listing);
        }

        private async Task<IList<string>> GetUserRole(User currentUser)
        {
            return await _userManager.GetRolesAsync(currentUser);
        }

        private static ExpressionStarter<Listing> BuildConditions(FilterCriteria filterCriteria)
        {
            var filterConditions = PredicateBuilder.New<Listing>(true);

            filterConditions.And(l => l.StatusId == 1);

            string address = filterCriteria.Address;
            if (!string.IsNullOrEmpty(address))
            {
                filterConditions.And(l => l.City.Equals(address) || l.AddressLine1.Equals(address) || l.Zip.Equals(address));
            }

            if (filterCriteria.MaxAge.HasValue)
            {
                // specify maximum house age.
                filterConditions.And(l => DateTimeOffset.Now.Year - l.BuiltYear <= filterCriteria.MaxAge);
            }

            if (filterCriteria.MaxPrice.HasValue)
            {
                // specify max price.
                filterConditions.And(l => l.StatingPrice <= filterCriteria.MaxPrice);
            }

            if (filterCriteria.MinPrice.HasValue)
            {
                // specify min price.
                filterConditions.And(l => l.StatingPrice >= filterCriteria.MinPrice);
            }

            if (filterCriteria.MinSize.HasValue)
            {
                filterConditions.And(l => l.Size >= filterCriteria.MinSize);
            }

            return filterConditions;
        }

    }
}
