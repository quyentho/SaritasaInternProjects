using FluentValidation;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordModelValidator()
        {
            RuleFor(x => x.NewPassword).NotNull();
            RuleFor(x => x.ConfirmPassword).NotNull().Equal(x => x.NewPassword).WithMessage("Password confirm  not match!");
        }
    }
}
