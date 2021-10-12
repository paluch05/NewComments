using FluentValidation;
using Kainos.Comments.Application.Model;

namespace Kainos.Comments.Functions.Validators
{
    public class AddCommentRequestValidator : AbstractValidator<AddCommentRequest>
    {
        public AddCommentRequestValidator()
        {
            RuleFor(x => x.Text).MaximumLength(200)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.Author).MaximumLength(50)
                .NotNull()
                .NotEmpty();
        }
    }
}