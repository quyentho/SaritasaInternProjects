using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnrealEstate.Infrastructure;
using UnrealEstate.Infrastructure.Repository;

namespace UnrealEstate.Business.Listing.Repository
{
    public class ListingRepository : BaseRepository<Infrastructure.Models.Listing>, IListingRepository
    {
        private readonly UnrealEstateDbContext _context;

        public ListingRepository(UnrealEstateDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Infrastructure.Models.Listing> GetListingByIdAsync(int listingId)
        {
            var listing = await _context
                .Listings
                .Include(l => l.Comments).ThenInclude(c => c.User)
                .Include(l => l.Favorites)
                .Include(l => l.Bids)
                .Include(l => l.ListingPhoTos)
                .Include(l => l.Status)
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == listingId);

            return listing;
        }

        public async Task<List<Infrastructure.Models.Listing>> GetListingsAsync()
        {
            var listings = await _context.Listings
                .Include(l => l.Comments)
                .Include(l => l.Favorites)
                .Include(l => l.Bids)
                .Include(l => l.ListingPhoTos)
                .Include(l => l.User)
                .Include(l => l.Status)
                .ToListAsync();

            return listings;
        }
    }
}