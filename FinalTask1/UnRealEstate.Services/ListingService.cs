using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.Repositories;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;

namespace UnrealEstate.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingRepository _listingRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public ListingService(IListingRepository listingRepository, UserManager<User> userManager, IMapper mapper)
        {
            _listingRepository = listingRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<bool> AddOrRemoveFavoriteUserAsync(int listingId, string userId)
        {
            Listing listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);
            GuardClauses.HasValue(listingFromDb, "listing id"); // null check.

            var favorite = listingFromDb.Favorites.Where(f => f.UserId == userId).FirstOrDefault();

            bool isFavorite = SetFavoriteState(listingId, userId, listingFromDb, favorite);

            await _listingRepository.UpdateListingAsync(listingFromDb);

            return isFavorite;
        }

        /// <inheritdoc/>
        public async Task EnableListingAsync(User currentUser, int listingId)
        {
            Listing listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);

            await ValidateForAdminAction(currentUser, listingFromDb, (int)Status.Disable);

            await Enable(listingFromDb);
        }

        /// <inheritdoc/>
        public async Task DisableListingAsync(User currentUser, int listingId)
        {
            Listing listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);

            await ValidateForAdminAction(currentUser, listingFromDb, (int)Status.Active);

            await Disable(listingFromDb);
        }



        /// <inheritdoc/>
        public async Task<List<ListingResponseViewModel>> GetListingsAsync()
        {
            List<Listing> listings = await _listingRepository.GetListingsAsync();

            List<ListingResponseViewModel> listingViewModels = MapListingsToViewModels(listings);

            return listingViewModels;
        }



        /// <inheritdoc/>
        public async Task<ListingResponseViewModel> GetListingAsync(int listingId)
        {
            Listing listing = await _listingRepository.GetListingByIdAsync(listingId);

            ListingResponseViewModel listingViewModel = _mapper.Map<ListingResponseViewModel>(listing);

            return listingViewModel;
        }

        /// <inheritdoc/>
        public async Task<List<ListingResponseViewModel>> GetActiveListingWithFilterAsync(FilterCriteriaRequestViewModel filterCriteria)
        {
            ExpressionStarter<Listing> filterConditions = BuildConditions(filterCriteria);

            List<Listing> listingsFiltered = await _listingRepository.GetListingsWithFilterAsync(filterConditions);

            List<ListingResponseViewModel> listingViewModels = MapListingsToViewModels(listingsFiltered);

            return listingViewModels;
        }

        /// <inheritdoc/>
        public async Task CreateListingAsync(ListingRequestViewModel listingViewModel, string userId)
        {
            Listing listing = _mapper.Map<Listing>(listingViewModel);

            listing.UserId = userId;

            await _listingRepository.AddListingAsync(listing);
        }

        /// <inheritdoc/>
        public async Task EditListingAsync(User currentUser, ListingRequestViewModel listingViewModel,int listingId)
        {
            var listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);

            await ValidateForAdminOrAuthorAction(currentUser, listingFromDb);

            _mapper.Map(listingViewModel, listingFromDb);

            await _listingRepository.UpdateListingAsync(listingFromDb);
        }

        private async Task Disable(Listing listingFromDb)
        {
            listingFromDb.StatusId = (int)Status.Disable;

            await _listingRepository.UpdateListingAsync(listingFromDb);
        }

        private async Task ValidateForAdminOrAuthorAction(User currentUser, Listing listingFromDb)
        {
            IList<string> userRole = await GetUserRole(currentUser);

            GuardClauses.HasValue(listingFromDb, "listing");

            GuardClauses.IsAuthorOrAdmin(currentUser.Id, listingFromDb.UserId, userRole.First());
        }

        private async Task<IList<string>> GetUserRole(User currentUser)
        {
            return await _userManager.GetRolesAsync(currentUser);
        }

        private static ExpressionStarter<Listing> BuildConditions(FilterCriteriaRequestViewModel filterCriteria)
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

        private static bool SetFavoriteState(int listingId, string userId, Listing listingFromDb, Favorite favorite)
        {
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

            return isFavorite;
        }

        private async Task Enable(Listing listingFromDb)
        {
            listingFromDb.StatusId = (int)Status.Active;

            await _listingRepository.UpdateListingAsync(listingFromDb);
        }

        private async Task ValidateForAdminAction(User currentUser, Listing listingFromDb, int statusId)
        {
            IList<string> userRole = await GetUserRole(currentUser);

            GuardClauses.HasValue(listingFromDb, "listing id");
            GuardClauses.IsAdmin(userRole.First());
            GuardClauses.IsValidStatus(listingFromDb.StatusId, statusId);
        }

        private List<ListingResponseViewModel> MapListingsToViewModels(List<Listing> listings)
        {
            return _mapper.Map<List<ListingResponseViewModel>>(listings);
        }
    }
}
