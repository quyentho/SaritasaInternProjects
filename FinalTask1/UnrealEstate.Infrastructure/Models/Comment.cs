﻿using System;

namespace UnrealEstate.Infrastructure.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public int ListingId { get; set; }

        public string UserId { get; set; }

        public Listing Listing { get; set; }

        public ApplicationUser User { get; set; }
    }
}