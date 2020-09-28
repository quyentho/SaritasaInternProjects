namespace UnrealEstate.Models.Models
{
    public class ListingPhoto
    {
        public int Id { get; set; }

        public int ListingId { get; set; }

        public string PhotoUrl { get; set; }

        public UnrealEstate.Models.Models.Listing Listing { get; set; }
    }
}