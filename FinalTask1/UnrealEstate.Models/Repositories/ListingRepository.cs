using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnrealEstate.Models.Models;

namespace UnrealEstate.Models.Repositories
{

    public class ListingRepository : IListingRepository
    {
        private readonly UnrealEstateDbContext _context;

        public ListingRepository(UnrealEstateDbContext context)
        {
            _context = context;
        }

        public async Task<Listing> GetListingByIdAsync(int listingId)
        {
            var listing = await _context
                .Listings
                .Include(l => l.Comments).ThenInclude(c=>c.User)
                .Include(l => l.Favorites)
                .Include(l => l.Bids)
                .Include(l=>l.ListingPhoTos)
                .Include(l=>l.Status)
                .Include(l=>l.User)
                .FirstOrDefaultAsync(l => l.Id == listingId);

            return listing;
        }

        public async Task<List<Listing>> GetListingsAsync()
        {
            var listings = await _context.Listings
                .Include(l => l.Comments)
                .Include(l => l.Favorites)
                .Include(l => l.Bids)
                .Include(l => l.ListingPhoTos)
                .Include(l => l.User)
                .Include(l=>l.Status)
                .ToListAsync();

            return listings;
        }

        public async Task AddListingAsync(Listing listing)
        {

            await _context.Listings.AddAsync(listing);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateListingAsync(Listing listing)
        {
            var oldListing = await _context.Listings.FindAsync(listing.Id);
            _context.Entry<Listing>(oldListing).CurrentValues.SetValues(listing);

            await _context.SaveChangesAsync();
        }
    }
}
