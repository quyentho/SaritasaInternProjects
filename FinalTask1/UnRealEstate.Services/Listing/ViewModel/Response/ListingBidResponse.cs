﻿using System;
using System.ComponentModel;

namespace UnrealEstate.Models.ViewModels.ResponseViewModels
{
    public class ListingBidResponse
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        [DisplayName("Created At")] public DateTimeOffset CreatedAt { get; set; }

        public string Comment { get; set; }
    }
}