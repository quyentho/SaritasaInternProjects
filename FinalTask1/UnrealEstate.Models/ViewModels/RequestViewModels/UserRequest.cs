using System;

namespace UnrealEstate.Models.ViewModels.RequestViewModels
{
    public class UserRequest
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset BirthDay { get; set; }

        public string PhoneNumber { get; set; }
    }
}
