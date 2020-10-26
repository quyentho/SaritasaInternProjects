using System.Collections.Generic;

namespace UnrealEstate.Infrastructure.Models
{
    public class ListingStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Listing> Listings { get; set; }
    }

    public enum Status
    {
        Active = 1,
        Disabled = 2,
        Canceled = 3,
        Sold = 4,
        NotAvailable = 5
    }
}