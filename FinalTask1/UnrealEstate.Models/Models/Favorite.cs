namespace UnrealEstate.Models
{
    public class Favorite
    {
        public int ListingId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public Listing Listing { get; set; }
    }
}