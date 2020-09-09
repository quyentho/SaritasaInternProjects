using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.Models.ViewModels.RequestViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
