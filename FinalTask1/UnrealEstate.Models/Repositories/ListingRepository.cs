﻿using Microsoft.EntityFrameworkCore;
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

        public Task<List<Listing>> GetListingsWithFilterAsync(Expression<Func<Listing, bool>> filterConditions)
        {
            return  _context.Listings
                                    .Include(l=>l.Comments)
                                    .Where(filterConditions)
                                    .ToListAsync();
        }

        public async Task<Listing> GetListingByIdAsync(int listingId)
        {
            return await _context
                .Listings
                .Include(l=>l.Comments)
                .Include(l=>l.Favorites)
                .FirstOrDefaultAsync(l=>l.Id == listingId);
        }

        public Task<List<Listing>> GetListingsAsync()
        {
            return _context.Listings.ToListAsync();
        }

        public async Task AddListingAsync(Listing listing)
        {
            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateListingAsync(Listing listing)
        {
           var oldListing = _context.Listings.Find(listing.Id);
            _context.Entry<Listing>(oldListing).CurrentValues.SetValues(listing);

            await _context.SaveChangesAsync();
        }
    }
}
