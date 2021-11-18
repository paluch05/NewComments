using System;
using System.Threading.Tasks;
using System.Web.Http;
using FluentAssertions;
using Kainos.Comments.Application.Model;
using Kainos.Comments.Application.Services;
using Kainos.Comments.Functions.Functions;
using Kainos.Comments.Functions.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TestProject1.Helpers;
using Xunit;

namespace TestProject1.FunctionsTests
{
    public class AddCommentFunctionTest
    {
        [Fact]
        public async Task ShouldReturnOkObject_WhenServiceResponsesHappily()
        {
            var serviceMock = new Mock<IExecutable<AddCommentRequest, AddCommentResponse>>();
            var addCommentResponse = new AddCommentResponse();
            serviceMock.Setup(_ => _.ExecuteAsync(It.IsAny<AddCommentRequest>()))
                .ReturnsAsync(addCommentResponse);
            var addCommentValidator = new AddCommentRequestValidator();

            var endpoint = new AddCommentFunction(serviceMock.Object, addCommentValidator);

            var request = new AddCommentRequest
            {
                Author = "anonim",
                Text = "blablabla"
            };

            var requestMock = HttpRequestHelper.CreateMockRequest(request);

            var result = await endpoint.AddCommentAsync(requestMock.Object, new NullLogger<AddCommentFunction>());

            result.Should().BeEquivalentTo(new OkObjectResult(addCommentResponse));

            serviceMock.Verify(_ => _.ExecuteAsync(It.IsAny<AddCommentRequest>()), Times.Once);
        }

        [Fact]
        public async Task ShouldReturnInternalServerErrorResponse_WhenServiceThrowsException()
        {
            var serviceMock = new Mock<IExecutable<AddCommentRequest, AddCommentResponse>>();
            serviceMock.Setup(_ => _.ExecuteAsync(It.IsAny<AddCommentRequest>()))
                .Throws<Exception>();
            var addCommentValidator = new AddCommentRequestValidator();

            var endpoint = new AddCommentFunction(serviceMock.Object, addCommentValidator);

            var request = new AddCommentRequest
            {
                Author = "anonim",
                Text = "blablabla"
            };

            var requestMock = HttpRequestHelper.CreateMockRequest(request);

            var response = await endpoint.AddCommentAsync(requestMock.Object, new NullLogger<AddCommentFunction>());

            response.Should().BeEquivalentTo(new InternalServerErrorResult());
            serviceMock.Verify(_ => _.ExecuteAsync(It.IsAny<AddCommentRequest>()), Times.Once());
        }

        [Theory]
        [InlineData(" \"author\": \"gggg\", \"text\": \"jdkshfhuwhf\"}")]
        [InlineData(" {\"author\": \"gggg\", \"text\": \"jdkshfhuwhf\"")]
        [InlineData(" {\"author\": \"gggg\" \"text\": \"jdkshfhuwhf\"}")]
        [InlineData(" {\"author\" \"gggg\", \"text\": \"jdkshfhuwhf\"")]
        [InlineData(" {\"author\": \"gggg\", \"text\" \"jdkshfhuwhf\"")]
        public async Task ShouldReturnBadRequestWhenPassedWrongJson(string json)
        {
            var serviceMock = new Mock<IExecutable<AddCommentRequest, AddCommentResponse>>();
            serviceMock.Setup(_ => _.ExecuteAsync(It.IsAny<AddCommentRequest>()))
                .Throws<Exception>();

            var addCommentValidator = new AddCommentRequestValidator();

            var endpoint = new AddCommentFunction(serviceMock.Object, addCommentValidator);

            var requestMock = HttpRequestHelper.CreateMockRequest(json);
        
            var response = await endpoint.AddCommentAsync(requestMock.Object, new NullLogger<AddCommentFunction>());

            response.Should().BeOfType<BadRequestObjectResult>();
            response.Should().BeEquivalentTo(new BadRequestObjectResult(new { reason = "Wrong Json format." }));
        }
    }
}
