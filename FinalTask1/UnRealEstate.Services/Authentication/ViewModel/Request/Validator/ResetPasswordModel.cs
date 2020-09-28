using FluentValidation;
using UnrealEstate.Models.ViewModels.RequestViewModels;

namespace UnrealEstate.Services.Authentication.ViewModel.Request.Validator
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