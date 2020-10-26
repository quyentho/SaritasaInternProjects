using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.Business.Authentication.ViewModel.Request
{
    public class ForgotPasswordRequest
    {
        [Required] [EmailAddress] public string Email { get; set; }
    }
}