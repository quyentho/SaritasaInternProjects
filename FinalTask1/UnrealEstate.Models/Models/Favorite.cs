using UnrealEstate.Models.Models;

namespace UnrealEstate.Models
{
    public class Favorite
    {
        public int ListingId { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public Listing Listing { get; set; }
    }
}