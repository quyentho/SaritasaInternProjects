using FluentValidation;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class UserFilterCriteriaModelValidator : AbstractValidator<UserFilterCriteriaRequest>
    {
        public UserFilterCriteriaModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
