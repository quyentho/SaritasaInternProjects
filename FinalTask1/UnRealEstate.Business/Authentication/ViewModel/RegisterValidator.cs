using FluentValidation;

namespace UnrealEstate.Business.Authentication.ViewModel.Request.Validator
{
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotNull();

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
                .WithMessage("Confirm password does not match").When(x => x.ConfirmPassword != null);
        }
    }
}