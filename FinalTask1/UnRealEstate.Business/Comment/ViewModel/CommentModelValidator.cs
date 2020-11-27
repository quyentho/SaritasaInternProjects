using FluentValidation;

namespace UnrealEstate.Business.Comment.ViewModel
{
    public class CommentModelValidator : AbstractValidator<CommentRequest>
    {
        public CommentModelValidator()
        {
            RuleFor(x => x.Text).NotEmpty();

            RuleFor(x => x.ListingId).NotNull();
        }
    }
}