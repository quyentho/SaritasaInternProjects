using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
