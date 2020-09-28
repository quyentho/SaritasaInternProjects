using System;

namespace UnrealEstate.Models.Models
{
    public class Bid
    {
        public int Id { get; set; }

        public int ListingId { get; set; }

        public string UserId { get; set; }

        public decimal Price { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string Comment { get; set; }

        public Listing Listing { get; set; }

        public User User { get; set; }
    }
}
