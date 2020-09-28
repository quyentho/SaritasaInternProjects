using FluentValidation;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class ResetPasswordModel : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordModel()
        {
            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.Token).NotEmpty();

            RuleFor(x => x.NewPassword).NotEmpty();

            RuleFor(x => x.ConfirmPassword)
                        .Equal(x => x.NewPassword);
        }
    }
}
