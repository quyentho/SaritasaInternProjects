using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.Models.ViewModels.RequestViewModels
{
    public class RegisterRequest
    {
        [EmailAddress] public string Email { get; set; }

        [DataType(DataType.Password)] public string Password { get; set; }

        [DataType(DataType.Password)] public string ConfirmPassword { get; set; }
    }
}