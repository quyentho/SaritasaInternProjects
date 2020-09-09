using System;
using System.Collections.Generic;
using UnrealEstate.Models.Models;

namespace UnrealEstate.Models
{
    public class Listing
    {
        public int Id { get; set; }

        public int StatusId { get; set; } = 1;

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

        public string Description { get; set; }

        public User User { get; set; }

        public ListingStatus Status { get; set; }

        public List<ListingPhoto> ListingPhoTos { get; set; }

        public List<ListingNote> ListingNotes { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Favorite> Favorites { get; set; }
    }
}