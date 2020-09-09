using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public int ListingId { get; set; }

        public string UserEmail { get; set; }

        public string UserId { get; set; }
    }
}
