using System;
using System.Threading.Tasks;
using System.Web.Http;
using FluentAssertions;
using Kainos.Comments.Application.Model.Domain;
using Kainos.Comments.Application.Services;
using Kainos.Comments.Functions.Functions;
using Kainos.Comments.Functions.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using TestProject1.Helpers;
using Xunit;

namespace TestProject1.FunctionsTests
{
    public class UpdateCommentByIdFunctionTest
    {
        [Fact]
        public async Task ShouldReturnOkObject_WhenServiceResponsesHappily()
        {
            var serviceMock = new Mock<IExecutable<UpdateCommentRequest, UpdateCommentResponse>>();
            var updateCommentResponse = new UpdateCommentResponse();
            serviceMock.Setup(_ => _.ExecuteAsync(It.IsAny<UpdateCommentRequest>()))
                .ReturnsAsync(updateCommentResponse);

            var updateValidator = new UpdateCommentRequestValidator();
            var endpoint = new UpdateCommentByIdFunction(serviceMock.Object, updateValidator);

            var request = new UpdateCommentRequest
            {
                Id = "053f7305-6d1c-42e7-b6ec-0727ab1f1c33",
                Text = "blablabla"
            };

            var requestMock = HttpRequestHelper.CreateMockRequest(request);

            var result = await endpoint.UpdateCommentByIdAsync(requestMock.Object, new NullLogger<UpdateCommentByIdFunction>(), request.Id);

            result.Should().BeEquivalentTo(new OkObjectResult(updateCommentResponse));

            serviceMock.Verify(_ => _.ExecuteAsync(It.IsAny<UpdateCommentRequest>()), Times.Once);
        }

        [Fact]
        public async Task ShouldReturnInternalServerErrorResponse_WhenServiceThrowsException()
        {
            var serviceMock = new Mock<IExecutable<UpdateCommentRequest, UpdateCommentResponse>>();
            serviceMock.Setup(_ => _.ExecuteAsync(It.IsAny<UpdateCommentRequest>()))
                .Throws<Exception>();

            var updateValidator = new UpdateCommentRequestValidator();
            var endpoint = new UpdateCommentByIdFunction(serviceMock.Object, updateValidator);

            var request = new UpdateCommentRequest
            {
                Id = "053f7305-6d1c-42e7-b6ec-0727ab1f1c33",
                Text = "blablabla"
            };

            var requestMock = HttpRequestHelper.CreateMockRequest(request);

            var response = await endpoint.UpdateCommentByIdAsync(requestMock.Object, new NullLogger<UpdateCommentByIdFunction>(), request.Id);

            response.Should().BeEquivalentTo(new InternalServerErrorResult());
            serviceMock.Verify(_ => _.ExecuteAsync(It.IsAny<UpdateCommentRequest>()), Times.Once());
        }

        [Theory]
        [InlineData(" \"id\": \"gggg\", \"text\": \"jdkshfhuwhf\"}")]
        [InlineData(" {\"id\": \"gggg\", \"text\": \"jdkshfhuwhf\"")]
        [InlineData(" {\"id\": \"gggg\" \"text\": \"jdkshfhuwhf\"}")]
        [InlineData(" {\"id\" \"gggg\", \"text\": \"jdkshfhuwhf\"")]
        [InlineData(" {\"id\": \"gggg\", \"text\" \"jdkshfhuwhf\"")]
        public async Task ShouldReturnBadRequest_WhenPassedWrongJson(string json)
        {
            var serviceMock = new Mock<IExecutable<UpdateCommentRequest, UpdateCommentResponse>>();
            serviceMock.Setup(_ => _.ExecuteAsync(It.IsAny<UpdateCommentRequest>()))
                .Throws<Exception>();

            var updateValidator = new UpdateCommentRequestValidator();
            var endpoint = new UpdateCommentByIdFunction(serviceMock.Object, updateValidator);
            var request = JsonConvert.SerializeObject(json);

            var requestMock = HttpRequestHelper.CreateMockRequest(request);

            var response = await endpoint.UpdateCommentByIdAsync(requestMock.Object, new NullLogger<AddCommentFunction>(), It.IsAny<Guid>().ToString());

            response.Should().BeOfType<BadRequestObjectResult>();
            response.Should().BeEquivalentTo(new BadRequestObjectResult(new { reason = "Wrong Json format." }));
        }
    }
}
