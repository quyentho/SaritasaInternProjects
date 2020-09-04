using LinqKit;
using System;
using System.Collections.Generic;
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

        public void DisableListing(int listingId)
        {
            var listingFromDb = _listingRepository.GetListingById(listingId);

            _ = listingFromDb ?? throw new ArgumentOutOfRangeException(paramName: "listing id", message: $"Not found Listing with id {listingId}");

            listingFromDb.StatusId = 3; // respresents cancel status.

            _listingRepository.UpdateListing(listingFromDb);
        }

        /// <summary>
        /// Gets list of listings.
        /// </summary>
        /// <returns>List of listing</returns>
        public List<Listing> GetListings() => _listingRepository.GetListings();

        public Listing GetListing(int listingId)
        {
            return _listingRepository.GetListingById(listingId);
        }

        public List<Listing> GetActiveListingWithFilter(FilterCriteria filterCriteria)
        {
            ExpressionStarter<Listing> filterConditions = BuildConditions(filterCriteria);

            return _listingRepository.GetListingsWithFilter(filterConditions);
        }

        public void CreateListing(Listing listing)
        {
            _listingRepository.AddListing(listing);
        }

        public void EditListing(Listing editedListing)
        {
            _listingRepository.UpdateListing(editedListing);
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
