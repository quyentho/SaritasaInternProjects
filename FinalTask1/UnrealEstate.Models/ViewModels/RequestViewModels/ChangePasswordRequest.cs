using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.Models.ViewModels.RequestViewModels
{
    public class ChangePasswordRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
