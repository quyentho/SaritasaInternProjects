using System;
using System.Collections.Generic;
using UnrealEstate.Models.ViewModels.ResponseViewModels;

namespace UnrealEstate.Models.ViewModels
{
    public class UserResponse
    {
        public string FirstName { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTimeOffset BirthDay { get; set; }

        public List<ListingResponse> ListingsCreated { get; set; }

        public List<ListingNoteResponse> ListingNotes { get; set; }

        public List<CommentResponse> Comments { get; set; }

        public List<ListingResponse> FavoriteListings { get; set; }

        public List<BidResponse> Bids { get; set; }
    }
}
