namespace UnrealEstate.Models.Models
{
    public class Favorite
    {
        public int ListingId { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Listing Listing { get; set; }
    }
}