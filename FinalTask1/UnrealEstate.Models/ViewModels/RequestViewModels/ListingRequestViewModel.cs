using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.Models.ViewModels.RequestViewModels
{
    public class ListingRequestViewModel
    {
        public string Zip { get; set; }

        public string City { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public uint BuiltYear { get; set; }

        public uint Beds { get; set; }

        public double Size { get; set; }

        [Required]
        public decimal StatingPrice { get; set; }

        [Required]
        public DateTimeOffset DueDate { get; set; }

        public string Description { get; set; }

        public List<ListingPhotoRequestViewModel> ListingPhoTos { get; set; }

        public List<ListingNoteRequestViewModel> ListingNotes { get; set; }
    }
}
