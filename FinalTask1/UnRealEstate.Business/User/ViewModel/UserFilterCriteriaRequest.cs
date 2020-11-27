using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.Business.User.ViewModel
{
    public class UserFilterCriteriaRequest
    {
        [EmailAddress] public string Email { get; set; }

        public string Name { get; set; }

        public uint? Offset { get; set; }

        public uint? Limit { get; set; }

        public string OrderBy { get; set; }
    }
}