using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.Models.ViewModels.RequestViewModels
{
    public class CommentRequestViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
