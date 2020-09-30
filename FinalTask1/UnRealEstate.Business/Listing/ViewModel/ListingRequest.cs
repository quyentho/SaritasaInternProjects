using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace UnrealEstate.Business.Listing.ViewModel
{
    public class ListingRequest
    {
        public ListingRequest()
        {
            ListingPhoTos = new List<IFormFile>();
            ListingNotes = new List<ListingNoteRequest>();
        }

        public string Zip { get; set; }

        public string City { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public uint? BuiltYear { get; set; }

        public uint? Beds { get; set; }

        public double? Size { get; set; }

        public decimal StatingPrice { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public string Description { get; set; }

        public List<IFormFile> ListingPhoTos { get; set; }

        public List<ListingNoteRequest> ListingNotes { get; set; }
    }
}