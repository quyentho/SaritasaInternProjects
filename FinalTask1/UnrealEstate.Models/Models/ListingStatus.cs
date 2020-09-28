using System.Collections.Generic;

namespace UnrealEstate.Models.Models
{
    public class ListingStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<UnrealEstate.Models.Models.Listing> Listings { get; set; }
    }

    public enum Status
    {
        Active = 1,
        Disable = 2,
        Canceled = 3,
        Sold = 4,
        NotAvailable = 5
    }
}
