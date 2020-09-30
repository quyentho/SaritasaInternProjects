using FluentValidation;

namespace UnrealEstate.Business.User.ViewModel
{
    public class UserFilterCriteriaValidator : AbstractValidator<UserFilterCriteriaRequest>
    {
        public UserFilterCriteriaValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}