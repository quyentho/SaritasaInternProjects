using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.Models.ViewModels.RequestViewModels
{
    public class CommentRequest
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public int ListingId { get; set; }
    }
}
