using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.Models.ViewModels.RequestViewModels
{
    public class ResetPasswordRequestViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
