using FluentValidation;

namespace UnrealEstate.Business.User.ViewModel.Request.Validator
{
    public class UserFilterCriteriaModelValidator : AbstractValidator<UserFilterCriteriaRequest>
    {
        public UserFilterCriteriaModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}