using LinqKit;
using Microsoft.AspNetCore.Authorization;
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

        /// <inheritdoc/>
        public async Task<bool> AddOrRemoveFavoriteUserAsync(int listingId, string userId)
        {
            GuardClauses.IsNotNull(userId, "user id");
            GuardClauses.IsNotNull(listingId, "user id");

            var listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);
            
            GuardClauses.HasValue(listingFromDb, "listing id");

            var favorite = listingFromDb.Favorites.Where(f => f.UserId == userId).FirstOrDefault();

            bool isFavorite;
            if (favorite is null)
            {
                isFavorite = true;
                listingFromDb.Favorites.Add(new Favorite() { ListingId = listingId, UserId = userId });
            }
            else // User already favorited this listing.
            {
                isFavorite = false;
                listingFromDb.Favorites.Remove(favorite);
            }

            await _listingRepository.UpdateListingAsync(listingFromDb);

            return isFavorite;
        }

        /// <inheritdoc/>
        public async Task EnableListingAsync(int listingId)
        {
            GuardClauses.IsNotNull(listingId, "listing id");

            var listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);

            GuardClauses.HasValue(listingFromDb, "listing id");

            GuardClauses.IsValidStatus(listingFromDb.StatusId, (int)Status.Disable);

            listingFromDb.StatusId = (int)Status.Active;

            await _listingRepository.UpdateListingAsync(listingFromDb);
        }

        /// <inheritdoc/>
        public async Task DisableListingAsync(User currentUser,int listingId)
        {
            GuardClauses.IsNotNull(currentUser, "current user");
            GuardClauses.IsNotNull(listingId, "listing id");

            IList<string> userRole = await GetUserRole(currentUser);
            
            var listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);

            GuardClauses.IsAdmin(userRole.First());

            GuardClauses.HasValue(listingFromDb, "listing id");

            GuardClauses.IsValidStatus(listingFromDb.StatusId, (int)Status.Active);

            listingFromDb.StatusId = (int)Status.Disable;

            await _listingRepository.UpdateListingAsync(listingFromDb);
        }

        /// <inheritdoc/>
        public Task<List<Listing>> GetListingsAsync() => _listingRepository.GetListingsAsync();

        public Task<Listing> GetListingAsync(int listingId)
        {
            GuardClauses.IsNotNull(listingId, "listing id");

            return _listingRepository.GetListingByIdAsync(listingId);
        }

        /// <inheritdoc/>
        public Task<List<Listing>> GetActiveListingWithFilterAsync(FilterCriteriaModel filterCriteria)
        {
            ExpressionStarter<Listing> filterConditions = BuildConditions(filterCriteria);

            return _listingRepository.GetListingsWithFilterAsync(filterConditions);
        }

        /// <inheritdoc/>
        public async Task CreateListingAsync(Listing listing)
        {
            GuardClauses.IsNotNull(listing, "listing");

            await _listingRepository.AddListingAsync(listing);
        }

        /// <inheritdoc/>
        public async Task EditListingAsync(User currentUser,Listing listing)
        {
            GuardClauses.IsNotNull(currentUser, "current user");
            GuardClauses.IsNotNull(listing, "listing");


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

        private static ExpressionStarter<Listing> BuildConditions(FilterCriteriaModel filterCriteria)
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
