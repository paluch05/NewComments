using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using FluentAssertions;
using Kainos.Comments.Application.Model;
using Kainos.Comments.Application.Model.Domain;
using Kainos.Comments.Application.Services;
using Kainos.Comments.Functions.Functions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TestProject1.Helpers;
using Xunit;

namespace TestProject1.FunctionsTests
{
    public class GetAllCommentsFunctionTest
    {
        [Fact]
        public async Task ShouldReturnOkObject_WhenServiceResponsesHappily()
        {
            var serviceMock = new Mock<IExecutable<GetAllCommentsRequest, GetAllCommentsResponse>>();
            var getAllCommentsResponse = new GetAllCommentsResponse();
            serviceMock.Setup(_ => _.ExecuteAsync(It.IsAny<GetAllCommentsRequest>()))
                .ReturnsAsync(getAllCommentsResponse);
           
            var endpoint = new GetAllCommentsFunction(serviceMock.Object);

            var request = new GetAllCommentsRequest();

            var requestMock = HttpRequestHelper.CreateMockRequest(request);

            var result = await endpoint.GetAllCommentsAsync(requestMock.Object, new NullLogger<GetAllCommentsFunction>());

            result.Should().BeEquivalentTo(new OkObjectResult(new { result = getAllCommentsResponse }));
            serviceMock.Verify(_ => _.ExecuteAsync(It.IsAny<GetAllCommentsRequest>()), Times.Once);
        }

        [Fact]
        public async Task ShouldReturnInternalServerErrorResponse_WhenServiceThrowsException()
        {
            var serviceMock = new Mock<IExecutable<GetAllCommentsRequest, GetAllCommentsResponse>>();
            serviceMock.Setup(_ => _.ExecuteAsync(It.IsAny<GetAllCommentsRequest>()))
                .Throws<Exception>();

            var endpoint = new GetAllCommentsFunction(serviceMock.Object);

            var request = new GetAllCommentsRequest();

            var requestMock = HttpRequestHelper.CreateMockRequest(request);

            var response = await endpoint.GetAllCommentsAsync(requestMock.Object, new NullLogger<AddCommentFunction>());

            response.Should().BeEquivalentTo(new InternalServerErrorResult());
            serviceMock.Verify(_ => _.ExecuteAsync(It.IsAny<GetAllCommentsRequest>()), Times.Once());
        }
    }
}
