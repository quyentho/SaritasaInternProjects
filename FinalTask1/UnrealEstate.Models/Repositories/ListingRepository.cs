using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace UnrealEstate.Models.Repositories
{
    public class ListingRepository : IListingRepository
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

        public Listing GetListingById(int listingId)
        {
            return _context.Listings.Find(listingId);
        }

        public List<Listing> GetListings()
        {
            return _context.Listings.ToList();
        }

        public void AddListing(Listing listing)
        {
            _context.Listings.Add(listing);
        }

        public void UpdateListing(int id)
        {
            throw new ArgumentOutOfRangeException();
        }
    }
}
