using System;

namespace UnrealEstate.Models
{
    public class Listing
    {
        public int Id { get; set; }

        public int StatusId { get; set; }

        public int UserId { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public uint BuiltYear { get; set; }

        public uint Beds { get; set; }

        public double Size { get; set; }

        public double StatingPrice { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public string Description { get; set; }

    }
}