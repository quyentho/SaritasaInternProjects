﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset BirthDay { get; set; }

        public List<Listing> ListingsCreated { get; set; }

        public List<ListingNote> ListingNotes { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Listing> FavoriteListings { get; set; }
    }
}
