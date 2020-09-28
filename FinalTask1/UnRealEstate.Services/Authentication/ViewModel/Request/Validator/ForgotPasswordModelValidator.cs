using FluentValidation;
using UnrealEstate.Models.ViewModels.RequestViewModels;

namespace UnrealEstate.Services.Authentication.ViewModel.Request.Validator
{
    public class ForgotPasswordModelValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}