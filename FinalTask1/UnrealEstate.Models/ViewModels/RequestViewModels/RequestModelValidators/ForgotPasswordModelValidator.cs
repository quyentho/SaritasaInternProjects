using FluentValidation;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class ForgotPasswordModelValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
