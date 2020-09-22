using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnrealEstate.Models.ViewModels.ResponseViewModels;

namespace UnrealEstate.Models.ViewModels
{
    public class UserResponse
    {
        public string FirstName { get; set; }

        public string Email { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }          

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Birth Day")]
        public DateTimeOffset BirthDay { get; set; }

        [DisplayName("Listings Created")]
        public List<ListingResponse> ListingsCreated { get; set; }

        [DisplayName("Listing Notes")]
        public List<ListingNoteResponse> ListingNotes { get; set; }

        public List<CommentResponse> Comments { get; set; }

        [DisplayName("Favorite Listings")]
        public List<ListingResponse> FavoriteListings { get; set; }

        public List<BidResponse> Bids { get; set; }
    }
}
