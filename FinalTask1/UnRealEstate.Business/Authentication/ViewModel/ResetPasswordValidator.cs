using FluentValidation;

namespace UnrealEstate.Business.Authentication.ViewModel.Request.Validator
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.Token).NotEmpty();

            RuleFor(x => x.NewPassword).NotEmpty();

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword);
        }
    }
}