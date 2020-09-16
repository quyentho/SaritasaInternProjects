using System;
using System.Collections.Generic;

namespace UnrealEstate.Models.ViewModels.ResponseViewModels
{
    public class ListingResponse
    {
        public int Id { get; set; }

        public string StatusName { get; set; }

        public string UserEmail { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public uint BuiltYear { get; set; }

        public uint Beds { get; set; }

        public double Size { get; set; }

        public decimal CurrentHighestBidPrice { get; set; }

        public decimal StatingPrice { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public string Description { get; set; }

        public List<CommentResponse> Comments { get; set; }

        public List<ListingPhotoResponse> ListingPhoTos { get; set; }

        public List<ListingNoteResponse> ListingNotes { get; set; }

        public List<BidResponse> Bids { get; set; }
    }
}
