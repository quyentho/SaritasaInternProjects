using System;
using System.Collections.Generic;
using UnrealEstate.Models.ViewModels.ResponseViewModels;

namespace UnrealEstate.Models.ViewModels
{
    public class UserResponseViewModel
    {
        public string FirstName { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTimeOffset BirthDay { get; set; }

        public List<ListingResponseViewModel> ListingsCreated { get; set; }

        public List<ListingNoteResponseViewModel> ListingNotes { get; set; }

        public List<CommentResponseViewModel> Comments { get; set; }

        public List<ListingResponseViewModel> FavoriteListings { get; set; }

        public List<BidResponseViewModel> Bids { get; set; }
    }
}
