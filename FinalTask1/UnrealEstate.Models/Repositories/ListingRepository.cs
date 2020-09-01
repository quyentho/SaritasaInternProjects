using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace UnrealEstate.Models.Repositories
{
    class ListingRepository : IListingRepository
    {
        private readonly UnrealEstateDbContext _context;

        public ListingRepository(UnrealEstateDbContext context)
        {
            _context = context;
        }

        public List<Listing> GetListingsWithFilter(Expression<Func<Listing, bool>> filterConditions)
        {
            return _context.Listings.Where(filterConditions).ToList();
        }

        public Listing GetListing(int listingId)
        {
            return _context.Listings.Find(listingId);
        }

        public List<Listing> GetListings()
        {
            return _context.Listings.ToList();
        }
    }
}
