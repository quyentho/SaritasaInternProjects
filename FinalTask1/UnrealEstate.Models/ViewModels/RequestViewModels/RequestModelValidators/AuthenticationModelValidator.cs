using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.ViewModels.RequestViewModels.RequestModelValidators
{
    public class AuthenticationModelValidator : AbstractValidator<AuthenticationRequest>
    {
        public AuthenticationModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotNull();

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
                .WithMessage("Confirm password does not match").When(x=>x.ConfirmPassword != null);
        }
    }
}
