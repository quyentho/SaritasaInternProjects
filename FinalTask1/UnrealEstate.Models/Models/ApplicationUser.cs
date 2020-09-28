using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace UnrealEstate.Models.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset BirthDay { get; set; }

        public bool IsActive { get; set; }

        public List<Listing> Listings { get; set; }

        public List<ListingNote> ListingNotes { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Favorite> Favorites { get; set; }

        public List<Bid> Bids { get; set; }
    }
}