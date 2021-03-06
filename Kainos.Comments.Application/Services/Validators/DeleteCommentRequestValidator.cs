using System.Text.RegularExpressions;
using FluentValidation;
using Kainos.Comments.Application.Model.Domain;

namespace Kainos.Comments.Functions.Validators
{
    public class DeleteCommentRequestValidator : AbstractValidator<DeleteCommentByIdRequest>
    {
        public DeleteCommentRequestValidator()
        {
            RuleFor(x => x.Id).Length(36)
                .NotNull()
                .NotEmpty()
                .Matches(
                    new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$"));
        }
    }
}
