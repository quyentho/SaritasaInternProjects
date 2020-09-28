using FluentValidation;
using UnrealEstate.Models.ViewModels.RequestViewModels;

namespace UnrealEstate.Services.User.ViewModel.Request.Validator
{
    public class UserFilterCriteriaModelValidator : AbstractValidator<UserFilterCriteriaRequest>
    {
        public UserFilterCriteriaModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
