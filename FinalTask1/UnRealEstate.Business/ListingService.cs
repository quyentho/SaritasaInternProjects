using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnrealEstate.Business.Interfaces;
using UnrealEstate.Business.Utils;
using UnrealEstate.Models;
using UnrealEstate.Models.Models;
using UnrealEstate.Models.Repositories;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;

namespace UnrealEstate.Business
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

        // TODO: define exception for services.
        public async Task AddOrRemoveFavoriteAsync(int listingId, string userId)
        {
            Listing listing = await ValidateFavoriteAction(listingId);

            SetFavoriteState(listingId, userId, listing);

            await _listingRepository.UpdateListingAsync(listing);
        }



        /// <inheritdoc/>
        public async Task EnableListingAsync(User currentUser, int listingId)
        {
            Listing listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);

            await ValidateAction(currentUser, listingFromDb, (int)Status.Disable);

            await Enable(listingFromDb);
        }

        /// <inheritdoc/>
        public async Task DisableListingAsync(User currentUser, int listingId)
        {
            Listing listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);

            await ValidateAction(currentUser, listingFromDb, (int)Status.Active);

            await Disable(listingFromDb);
        }

        /// <inheritdoc/>
        public async Task<List<ListingResponse>> GetListingsAsync()
        {
            List<Listing> listings = await _listingRepository.GetListingsAsync();

            List<ListingResponse> listingViewModels = MapListingsToViewModels(listings);

            return listingViewModels;
        }

        /// <inheritdoc/>
        public async Task<ListingResponse> GetListingAsync(int listingId)
        {
            Listing listing = await _listingRepository.GetListingByIdAsync(listingId);

            ListingResponse listingViewModel = _mapper.Map<ListingResponse>(listing);

            return listingViewModel;
        }

        /// <inheritdoc/>
        public async Task<List<ListingResponse>> GetActiveListingsWithFilterAsync(ListingFilterCriteriaRequest filterCriteria)
        {

            List<Listing> listings = await _listingRepository.GetListingsAsync();

            listings = GetFilteredListings(filterCriteria, listings);

            List<ListingResponse> listingViewModels = MapListingsToViewModels(listings);

            return listingViewModels;
        }

        /// <inheritdoc/>
        public async Task CreateListingAsync(ListingRequest listingRequest, string userId)
        {
            Listing listing = await ProduceListing(listingRequest, userId);

            await _listingRepository.AddListingAsync(listing);
        }

        /// <inheritdoc/>
        public async Task EditListingAsync(User currentUser, ListingRequest listingRequest, int listingId)
        {
            Listing editedListing = await GetEditedListing(currentUser, listingRequest, listingId);

            await _listingRepository.UpdateListingAsync(editedListing);
        }

        /// <inheritdoc />
        public async Task MakeABid(int listingId, User currentUser, BidRequest bidRequestViewModel)
        {
            Listing listingToBid = await ValidateBidAction(listingId, bidRequestViewModel);

            AddBidOnListing(listingId, currentUser, bidRequestViewModel, listingToBid);

            await _listingRepository.UpdateListingAsync(listingToBid);
        }

        /// <inheritdoc/>
        public async Task DeletePhotoAsync(User currentUser, int listingId, int photoId)
        {
            GuardClauses.IsNotNull(currentUser, "User");

            var listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);

            GuardClauses.HasValue(listingFromDb, "listing id");

            var photo = listingFromDb.ListingPhoTos.FirstOrDefault(p => p.Id == photoId);

            GuardClauses.HasValue(photo, "photo id");

            if (File.Exists(photo.PhotoUrl))
            {
                File.Delete(photo.PhotoUrl);
            }

            listingFromDb.ListingPhoTos.Remove(photo);

            await _listingRepository.UpdateListingAsync(listingFromDb);
        }

        private void AddBidOnListing(int listingId, User currentUser, BidRequest bidRequestViewModel, Listing listingToBid)
        {
            listingToBid.CurrentHighestBidPrice = bidRequestViewModel.Price;

            Bid bid = _mapper.Map<Bid>(bidRequestViewModel);

            bid.ListingId = listingId;
            bid.CreatedAt = DateTimeOffset.Now;
            bid.UserId = currentUser.Id;

            listingToBid.Bids.Add(bid);
        }

        private async Task<Listing> ValidateBidAction(int listingId, BidRequest bidRequestViewModel)
        {
            var listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);

            GuardClauses.HasValue(listingFromDb, "listing id");

            GuardClauses.BidPriceHigherThanCurrentPrice(bidRequestViewModel.Price, listingFromDb.CurrentHighestBidPrice);

            return listingFromDb;
        }

        private async Task Disable(Listing listingFromDb)
        {
            listingFromDb.StatusId = (int)Status.Disable;

            await _listingRepository.UpdateListingAsync(listingFromDb);
        }

        private async Task<IList<string>> GetUserRole(User currentUser)
        {
            return await _userManager.GetRolesAsync(currentUser);
        }

        private static ExpressionStarter<Listing> BuildConditions(ListingFilterCriteriaRequest filterCriteria)
        {
            var filterConditions = PredicateBuilder.New<Listing>(true);

            filterConditions.And(l => l.StatusId == 1);

            string address = filterCriteria.Address;
            if (!string.IsNullOrEmpty(address))
            {
                filterConditions.And(l => l.City != null && l.City.Equals(address) || l.AddressLine1 != null && l.AddressLine1.Equals(address) || l.Zip != null && l.Zip.Equals(address));
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

        private static void SetFavoriteState(int listingId, string userId, Listing listingFromDb)
        {
            var favorite = listingFromDb.Favorites.FirstOrDefault(f => f.UserId == userId);

            if (favorite is null)
            {
                listingFromDb.Favorites.Add(new Favorite() { ListingId = listingId, UserId = userId });
            }
            else // User already favorited this listing.
            {
                listingFromDb.Favorites.Remove(favorite);
            }
        }

        private async Task Enable(Listing listingFromDb)
        {
            listingFromDb.StatusId = (int)Status.Active;

            await _listingRepository.UpdateListingAsync(listingFromDb);
        }

        private async Task ValidateAction(User currentUser, Listing listingFromDb, int validStatusId)
        {
            IList<string> userRole = await GetUserRole(currentUser);

            GuardClauses.HasValue(listingFromDb, "listing id");
            GuardClauses.IsAdmin(userRole.First());
            GuardClauses.IsValidStatus(listingFromDb.StatusId, validStatusId);
        }

        private List<ListingResponse> MapListingsToViewModels(List<Listing> listings)
        {
            return _mapper.Map<List<ListingResponse>>(listings);
        }

        private static List<Listing> GetFilteredListings(ListingFilterCriteriaRequest filterCriteria, List<Listing> listings)
        {
            IQueryable<Listing> result = FilterByConditions(filterCriteria, listings);

            result = result.SortBy(filterCriteria.OrderBy);

            result = result.FilterByRange((int?)filterCriteria.Offset, (int?)filterCriteria.Limit);

            return result.ToList();
        }

        private static IQueryable<Listing> FilterByConditions(ListingFilterCriteriaRequest filterCriteria, List<Listing> listings)
        {
            ExpressionStarter<Listing> filterConditions = BuildConditions(filterCriteria);

            IQueryable<Listing> result = listings.Where(filterConditions).AsQueryable();
            return result;
        }

        private static async Task<List<ListingPhoto>> GetUploadedListingPhotos(ListingRequest listingViewModel)
        {
            List<ListingPhoto> listingPhotos = new List<ListingPhoto>();
            foreach (var formFile in listingViewModel.ListingPhoTos)
            {
                if (formFile.Length > 0)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", Path.GetRandomFileName());

                    path = path.Replace(Path.GetExtension(path), Path.GetExtension(formFile.FileName));

                    listingPhotos.Add(new ListingPhoto { PhotoUrl = path });

                    using (var stream = System.IO.File.Create(path))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return listingPhotos;
        }

        private async Task<Listing> ProduceListing(ListingRequest listingViewModel, string userId)
        {
            Listing listing = _mapper.Map<Listing>(listingViewModel);

            listing.UserId = userId;
            listing.CurrentHighestBidPrice = listingViewModel.StatingPrice;
            listing.StatusId = (int)Status.Active;

            await AddUploadedPhotosIfExist(listingViewModel, listing);

            return listing;
        }

        private static async Task AddUploadedPhotosIfExist(ListingRequest listingViewModel, Listing listing)
        {
            bool hasPhotoUploaded = listingViewModel.ListingPhoTos.Count > 0;

            if (hasPhotoUploaded)
            {
                var existedPhotosNumber = listing.ListingPhoTos.Count;

                if (existedPhotosNumber >= 3)
                {
                    throw new InvalidOperationException("Number of photos cannot exceeds 3");
                }

                List<ListingPhoto> listingPhotos = await GetUploadedListingPhotos(listingViewModel);

                listing.ListingPhoTos.AddRange(listingPhotos);
            }
        }


        private async Task<Listing> GetEditedListing(User currentUser, ListingRequest listingViewModel, int listingId)
        {
            Listing listing = await ValidateEditAction(currentUser, listingId);

            _mapper.Map(listingViewModel, listing);

            await AddUploadedPhotosIfExist(listingViewModel, listing);

            return listing;
        }

        private async Task<Listing> ValidateEditAction(User currentUser, int listingId)
        {
            var listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);

            IList<string> userRole = await GetUserRole(currentUser);

            GuardClauses.HasValue(listingFromDb, "listing");

            GuardClauses.IsAuthorOrAdmin(currentUser.Id, listingFromDb.UserId, userRole.FirstOrDefault());

            return listingFromDb;
        }

        private async Task<Listing> ValidateFavoriteAction(int listingId)
        {
            Listing listingFromDb = await _listingRepository.GetListingByIdAsync(listingId);

            GuardClauses.HasValue(listingFromDb, "listing id");

            return listingFromDb;
        }
    }
}
