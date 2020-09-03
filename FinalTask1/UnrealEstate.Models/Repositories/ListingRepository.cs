using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
            _context.SaveChanges();
        }

        public void UpdateListing(Listing listing)
        {
            var listingFromDb = _context.Listings.Find(listing.Id);

            _ = listingFromDb ?? throw new ArgumentOutOfRangeException(paramName: "listing id", message: $"Not found listing with id:{listing.Id}");

            _context.Entry<Listing>(listingFromDb).CurrentValues.SetValues(listing);

            _context.SaveChanges();
        }

        public void Disable(int listingId)
        {
            throw new NotImplementedException();
        }
    }
}
