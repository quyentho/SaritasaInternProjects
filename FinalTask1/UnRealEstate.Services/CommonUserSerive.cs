using LinqKit;
using System;
using System.Collections.Generic;
using UnrealEstate.Models;
using UnrealEstate.Models.Models;
using UnrealEstate.Models.Repositories;

namespace UnRealEstate.Services
{
    public class CommonUserSerive
    {
        private readonly IListingRepository _listingRepository;
        private readonly IUserManager _userManager;

        public CommonUserSerive(IListingRepository listingRepository, IUserManager userManager)
        {
            _listingRepository = listingRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets list of listings.
        /// </summary>
        /// <returns>List of listing</returns>
        public List<Listing> GetListings()
        {
            return _listingRepository.GetListings();
        }

        public bool Login(string email, string password)
        {
            return _userManager.VerifyLogin(email, password);
        }

        public Listing GetListing(int listingId)
        {
            return _listingRepository.GetListingById(listingId);
        }

        public List<Listing> GetActiveListingWithFilter(FilterCriteria filterCriteria)
        {
            ExpressionStarter<Listing> filterConditions = BuildConditions(filterCriteria);

            return _listingRepository.GetListingsWithFilter(filterConditions);
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
