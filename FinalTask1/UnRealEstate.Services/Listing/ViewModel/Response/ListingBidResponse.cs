using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ViewModels.ResponseViewModels
{
    public class ListingBidResponse
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        [DisplayName("Created At")]
        public DateTimeOffset CreatedAt { get; set; }

        public string Comment { get; set; }
    }
}
