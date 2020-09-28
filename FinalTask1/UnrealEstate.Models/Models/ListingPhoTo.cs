namespace UnrealEstate.Infrastructure.Models
{
    public class ListingPhoto
    {
        public int Id { get; set; }

        public int ListingId { get; set; }

        public string PhotoUrl { get; set; }

        public Listing Listing { get; set; }
    }
}