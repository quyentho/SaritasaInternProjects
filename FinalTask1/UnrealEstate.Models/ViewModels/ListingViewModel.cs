using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ViewModels
{
    public class ListingViewModel
    {
       // public int Id { get; set; }

        public string StatusName { get; set; }

        public string UserId { get; set; }

        public string UserEmail { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public uint BuiltYear { get; set; }

        public uint Beds { get; set; }

        public double Size { get; set; }

        public decimal StatingPrice { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public string Description { get; set; }

        public List<Comment> Comments { get; set; }

        public List<ListingPhoTo> ListingPhoTos { get; set; }

        public List<ListingNote> ListingNotes { get; set; }
    }
}
