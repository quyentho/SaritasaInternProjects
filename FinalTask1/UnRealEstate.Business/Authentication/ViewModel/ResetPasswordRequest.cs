using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.Business.Authentication.ViewModel.Request
{
    public class ResetPasswordRequest
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required] public string Token { get; set; }

        [Required] public string NewPassword { get; set; }

        [Required] public string ConfirmPassword { get; set; }
    }
}