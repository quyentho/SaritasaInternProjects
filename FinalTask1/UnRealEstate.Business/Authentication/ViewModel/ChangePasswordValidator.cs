using FluentValidation;

namespace UnrealEstate.Business.Authentication.ViewModel.Request.Validator
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.NewPassword).NotNull();
            RuleFor(x => x.ConfirmPassword).NotNull().Equal(x => x.NewPassword)
                .WithMessage("Password confirm  not match!");
        }
    }
}