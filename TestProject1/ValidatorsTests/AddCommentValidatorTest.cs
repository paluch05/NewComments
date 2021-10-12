using System.Text;
using FluentAssertions;
using Kainos.Comments.Application.Model;
using Kainos.Comments.Functions.Validators;
using Xunit;

namespace TestProject1.ValidatorsTests
{
    public class AddCommentValidatorTest
    {
        [Theory]
        [InlineData(null, null, false)]
        [InlineData("gggg", null, false)]
        [InlineData(null, "kkkkkk", false)]
        [InlineData("yy", "dsdsa", true)]
        [InlineData("", "", false)]
        [InlineData("yy", "", false)]
        [InlineData("", "yy", false)]
        public void ShouldReturnBadRequest_WhenPassedNullValues(string author, string text, bool isValid)
        {
            var addCommentRequest = new AddCommentRequest
            {
                Author = author,
                Text = text
            };

            var validation = new AddCommentRequestValidator();

            var validate = validation.Validate(addCommentRequest);

            validate.IsValid.Should().Be(isValid);
        }

        [Fact]
        public void ShouldReturnBadRequest_WhenAuthorValueIsBiggerThanExpected()
        {
            var tooLongAuthor = TooLongString(350);
            var json = $"{{\"text\": \"jdljkslad\", \"author\": \"{tooLongAuthor}\"}}";

            var addCommentRequest = new AddCommentRequest
            {
                Author = tooLongAuthor,
                Text = "ccc"
            };

            var validation = new AddCommentRequestValidator();

            var validate = validation.Validate(addCommentRequest);

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
