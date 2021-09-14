using System.Text.RegularExpressions;
using FluentValidation;
using Kainos.Comments.Application.Model.Domain;

namespace Kainos.Comments.Functions.Validators
{
    public class UpdateCommentRequestValidator : AbstractValidator<UpdateCommentRequest>
    {
        public UpdateCommentRequestValidator()
        {
            RuleFor(x => x.Text).MaximumLength(200);
            RuleFor(x => x.Text).NotEmpty();
            RuleFor(x => x.Text).NotNull();
            RuleFor(x => x.Id).Matches(
                new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$"));
        }
    }
}