using System;
using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.Models.ViewModels.RequestViewModels
{
    public class AuthenticationRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset BirthDay { get; set; }

        public string PhoneNumber { get; set; }


    }
}
