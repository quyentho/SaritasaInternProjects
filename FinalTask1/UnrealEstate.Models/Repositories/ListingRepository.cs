using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UnrealEstate.Models.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly UnrealEstateDbContext _context;

        public ListingRepository(UnrealEstateDbContext context)
        {
            _context = context;
        }

        public Task<List<Listing>> GetListingsWithFilter(Expression<Func<Listing, bool>> filterConditions)
        {
            // TODO: Include Comments.
            return  _context.Listings.Where(filterConditions).ToListAsync();
        }

        public async Task<Listing> GetListingById(int listingId)
        {
            return await _context.Listings.FindAsync(listingId);
        }

        public Task<List<Listing>> GetListings()
        {
            return _context.Listings.ToListAsync();
        }

        public async Task AddListing(Listing listing)
        {
            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateListing(Listing listing)
        {
            _context.Entry<Listing>(listing).CurrentValues.SetValues(listing);

            await _context.SaveChangesAsync();
        }

        public async Task AddFavoriteUser(int listingId, string userId)
        {
            var listingFromDb = _context.Listings.Find(listingId);

            listingFromDb.Favorites.Add(new Favorite() { ListingId = listingId, UserId = userId });

            await _context.SaveChangesAsync();
        }
    }
}
