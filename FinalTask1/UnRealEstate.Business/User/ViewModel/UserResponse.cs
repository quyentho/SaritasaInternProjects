using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UnrealEstate.Business.Comment.ViewModel;
using UnrealEstate.Business.Listing.ViewModel;

namespace UnrealEstate.Business.User.ViewModel
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
        [DataType(DataType.Date)]
        public DateTimeOffset BirthDay { get; set; }

        [DisplayName("Listings Created")] 
        public List<ListingResponse> ListingsCreated { get; set; }

        [DisplayName("Listing Notes")] 
        public List<ListingNoteResponse> ListingNotes { get; set; }

        public List<CommentResponse> Comments { get; set; }

        [DisplayName("Favorite Listings")] 
        public List<ListingResponse> FavoriteListings { get; set; }

        public List<ListingBidResponse> Bids { get; set; }
    }
}