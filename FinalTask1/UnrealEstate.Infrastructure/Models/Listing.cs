using System;
using System.Collections.Generic;

namespace UnrealEstate.Infrastructure.Models
{
    public class Listing
    {
        public Listing()
        {
            Bids = new List<Bid>();
            ListingPhoTos = new List<ListingPhoto>();
        }

        public int Id { get; set; }

        public int StatusId { get; set; }

        public string UserId { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public uint BuiltYear { get; set; }

        public uint Beds { get; set; }

        public double Size { get; set; }

        public decimal StatingPrice { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public decimal CurrentHighestBidPrice { get; set; }

        public string Description { get; set; }

        public ApplicationUser User { get; set; }

        public ListingStatus Status { get; set; }

        public List<ListingPhoto> ListingPhoTos { get; set; }

        public List<ListingNote> ListingNotes { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Favorite> Favorites { get; set; }

        public List<Bid> Bids { get; set; }
    }
}