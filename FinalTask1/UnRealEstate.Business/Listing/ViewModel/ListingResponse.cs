using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnrealEstate.Business.Comment.ViewModel;

namespace UnrealEstate.Business.Listing.ViewModel
{
    public class ListingResponse
    {
        public int Id { get; set; }

        [DisplayName("Status")] public string StatusName { get; set; }

        [DisplayName("Owner Email")] public string UserEmail { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        [DisplayName("Address line 1")] public string AddressLine1 { get; set; }

        [DisplayName("Address line 2")] public string AddressLine2 { get; set; }

        [DisplayName("Built Year")] public uint BuiltYear { get; set; }

        public uint Beds { get; set; }

        public double Size { get; set; }

        [DisplayName("Current Highest Bid Price")]
        public decimal CurrentHighestBidPrice { get; set; }

        [DisplayName("Starting Price")] public decimal StatingPrice { get; set; }

        [DisplayName("Due Date")] public DateTimeOffset DueDate { get; set; }

        public string Description { get; set; }

        public List<CommentResponse> Comments { get; set; }

        public List<ListingPhotoResponse> ListingPhoTos { get; set; }
    }
}