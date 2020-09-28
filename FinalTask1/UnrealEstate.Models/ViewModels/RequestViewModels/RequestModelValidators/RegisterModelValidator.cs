﻿using FluentValidation;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class RegisterModelValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotNull();

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
                .WithMessage("Confirm password does not match").When(x => x.ConfirmPassword != null);
        }
    }
}
