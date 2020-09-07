using LinqKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.Models;
using UnrealEstate.Models.Repositories;

namespace UnrealEstate.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingRepository _listingRepository;

        public ListingService(IListingRepository listingRepository)
        {
            _listingRepository = listingRepository;
        }

        public async Task AddFavoriteUser(int listingId, string userId)
        {
            _ = userId ?? throw new ArgumentNullException(paramName: "user id", message: "User id cannot be null");

            var listingFromDb = await _listingRepository.GetListingById(listingId);

            _ = listingFromDb ?? throw new ArgumentOutOfRangeException(paramName: "listing id", message: "Listing is not exists");

            await _listingRepository.AddFavoriteUser(listingId, userId);
        }

        public async Task EnableListing(int listingId)
        {
            var listingFromDb = await _listingRepository.GetListingById(listingId);

            // TODO: Introduce Guard clause.
            _ = listingFromDb ?? throw new ArgumentOutOfRangeException(paramName: "listing id", message: $"Not found Listing with id {listingId}");

            if (listingFromDb.StatusId != (int)Status.Disable)
            {
                throw new InvalidOperationException("Listing status is not valid to enable.");
            }

            listingFromDb.StatusId = (int)Status.Active; // respresents Actived status.

            await _listingRepository.UpdateListing(listingFromDb);
        }

        // TODO: Add comments
        public async Task DisableListing(int listingId)
        {
            var listingFromDb = await _listingRepository.GetListingById(listingId);

            _ = listingFromDb ?? throw new ArgumentOutOfRangeException(paramName: "listing id", message: $"Not found Listing with id {listingId}");

            if (listingFromDb.StatusId != (int)Status.Active)
            {
                throw new InvalidOperationException("Listing status is not valid to disable.");
            }

            listingFromDb.StatusId = (int)Status.Disable; // respresents canceled status.

            await _listingRepository.UpdateListing(listingFromDb);
        }

        /// <summary>
        /// Gets list of listings.
        /// </summary>
        /// <returns>List of listing</returns>
        public Task<List<Listing>> GetListings() => _listingRepository.GetListings();

        public Task<Listing> GetListing(int listingId)
        {
            return _listingRepository.GetListingById(listingId);
        }

        public Task<List<Listing>> GetActiveListingWithFilter(FilterCriteria filterCriteria)
        {
            ExpressionStarter<Listing> filterConditions = BuildConditions(filterCriteria);

            return _listingRepository.GetListingsWithFilter(filterConditions);
        }

        public async Task CreateListing(Listing listing)
        {
            await _listingRepository.AddListing(listing);
        }

        public async Task EditListing(Listing listing)
        {
            var listingFromDb = _listingRepository.GetListingById(listing.Id);

            _ = listingFromDb ?? throw new ArgumentOutOfRangeException(paramName: "listing id", message: $"Not found listing with id:{listing.Id}");

            await _listingRepository.UpdateListing(listing);
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
