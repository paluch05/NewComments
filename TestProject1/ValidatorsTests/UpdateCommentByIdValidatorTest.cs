using System.Text;
using FluentAssertions;
using Kainos.Comments.Application.Model.Domain;
using Kainos.Comments.Functions.Validators;
using Xunit;

namespace TestProject1.ValidatorsTests
{
    public class UpdateCommentByIdValidatorTest
    {
        [Theory]
        [InlineData(null, null, false)]
        [InlineData("053f7305-6d1c-42e7-b6ec-0727ab1f1c33", null, false)]
        [InlineData(null, "kkkkkk", false)]
        [InlineData("053f7305-6d1c-42e7-b6ec-0727ab1f1c33", "dsdsa", true)]
        [InlineData("", "", false)]
        [InlineData("053f7305-6d1c-42e7-b6ec-0727ab1f1c33", "", false)]
        [InlineData("", "yy", false)]
        public void ShouldReturnBadRequest_WhenPassedNullValues(string id, string text, bool isValid)
        {
            var updateCommentRequest = new UpdateCommentRequest
            {
                Id = id,
                Text = text
            };

            var validation = new UpdateCommentRequestValidator();

            var validate = validation.Validate(updateCommentRequest);

            validate.IsValid.Should().Be(isValid);
        }

        [Fact]
        public void ShouldReturnBadRequest_WhenTextValueIsBiggerThanExpected()
        {
            var tooLongText = TooLongString(350);
            var json = $"{{\"text\": \"jdljkslad\", \"author\": \"{tooLongText}\"}}";

            var updateCommentRequest = new UpdateCommentRequest()
            {
                Id = "053f7305 - 6d1c - 42e7 - b6ec - 0727ab1f1c33",
                Text = tooLongText
            };

            var validation = new UpdateCommentRequestValidator();

            var validate = validation.Validate(updateCommentRequest);

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
