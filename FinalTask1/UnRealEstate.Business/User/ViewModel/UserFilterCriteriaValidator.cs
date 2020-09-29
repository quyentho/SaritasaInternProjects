using FluentValidation;

namespace UnrealEstate.Business.User.ViewModel.Request.Validator
{
    public class UserFilterCriteriaValidator : AbstractValidator<UserFilterCriteriaRequest>
    {
        public UserFilterCriteriaValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}