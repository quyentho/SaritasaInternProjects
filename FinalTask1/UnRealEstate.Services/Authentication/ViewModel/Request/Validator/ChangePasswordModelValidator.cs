using FluentValidation;
using UnrealEstate.Models.ViewModels.RequestViewModels;

namespace UnrealEstate.Services.Authentication.ViewModel.Request.Validator
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
