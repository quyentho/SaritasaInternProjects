using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ViewModels.RequestViewModels
{
    public class BidRequestViewModel
    {
        [Required]
        public decimal Price { get; set; }

        public string Comment { get; set; }
    }
}
