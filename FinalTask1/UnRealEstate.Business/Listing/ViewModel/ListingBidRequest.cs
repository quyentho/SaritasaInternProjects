﻿namespace UnrealEstate.Business.Listing.ViewModel
{
    public class ListingBidRequest
    {
        public int ListingId { get; set; }
        public decimal Price { get; set; }

        public string Comment { get; set; }
    }
}