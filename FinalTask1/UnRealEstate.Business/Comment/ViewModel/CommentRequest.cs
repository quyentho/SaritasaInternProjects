using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.Business.Comment.ViewModel
{
    public class CommentRequest
    {
        [Required] public string Text { get; set; }

        [Required] public int ListingId { get; set; }
    }
}