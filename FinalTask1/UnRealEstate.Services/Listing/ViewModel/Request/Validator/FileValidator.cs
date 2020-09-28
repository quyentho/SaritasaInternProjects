using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace UnrealEstate.Services.Listing.ViewModel.Request.Validator
{
    public class FileValidator : AbstractValidator<IFormFile>
    {
        public FileValidator()
        {
            RuleFor(x => x.ContentType).NotNull()
                .Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
                .WithMessage("File type is not image");
        }
    }
}