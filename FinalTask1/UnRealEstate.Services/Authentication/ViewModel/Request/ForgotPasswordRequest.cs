using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.Models.ViewModels.RequestViewModels
{
    public class ForgotPasswordRequest
    {
        [Required] [EmailAddress] public string Email { get; set; }
    }
}