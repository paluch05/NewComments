using System.Text;
using FluentAssertions;
using Kainos.Comments.Application.Model.Domain;
using Kainos.Comments.Functions.Validators;
using Xunit;

namespace TestProject1.ValidatorsTests
{
    public class DeleteCommentByIdValidatorTest
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("hjdkfhsdjkfdjskffh", false)]
        [InlineData("053f7305-6d1c-42e7-b6ec-0727ab1f1c33", true)]

        public void ShouldReturnBadRequest_WhenPassedIncorrectIdValues(string id, bool isValid)
        {
            var deleteCommentRequest = new DeleteCommentByIdRequest
            {
                Id = id,
            };

            var validation = new DeleteCommentRequestValidator();

            var validate = validation.Validate(deleteCommentRequest);

            validate.IsValid.Should().Be(isValid);
        }
        [Fact]
        public void ShouldReturnBadRequest_WhenIdValueIsBiggerThanExpected()
        {
            var tooLongId = TooLongString(37);
            var json = $"{{\"id\": \"{tooLongId}\"}}";

            var deleteCommentRequest = new DeleteCommentByIdRequest
            {
                Id = tooLongId
            };

            var validation = new DeleteCommentRequestValidator();

            var validate = validation.Validate(deleteCommentRequest);

            validate.IsValid.Should().BeFalse();
        }

        private static string TooLongString(int x)
        {
            var builder = new StringBuilder();
            var a = "a";
            for (var i = 0; i < x; i++)
            {
                builder.Append(a);
            }
            return builder.ToString();
        }
    }
}
