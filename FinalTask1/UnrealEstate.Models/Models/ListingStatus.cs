using System.Collections.Generic;

namespace UnrealEstate.Models
{
    public class ListingStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Listing> Listings { get; set; }
    }
}
