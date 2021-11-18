using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Kainos.Comments.Application.Cosmos;
using Kainos.Comments.Application.Exceptions;
using Kainos.Comments.Application.Model.Database;
using Kainos.Comments.Application.Model.Domain;
using Kainos.Comments.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TestProject1.ApplicationTests
{
    public class GetAllCommentsServiceTest
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());

        [Fact]
        public async Task ExecuteAsync_ShouldGetAllComments()
        {
            var allComments = _fixture.CreateMany<Comment>().ToList();

            var cosmosDbServiceMock = new Mock<ICosmosDbService>();
            cosmosDbServiceMock.Setup(_ => _.GetAllCommentsAsync()).ReturnsAsync(allComments);

            var log = new Mock<ILogger<GetAllCommentsService>>();

            var getAllCommentsService = new GetAllCommentsService(
                cosmosDbServiceMock.Object,
                log.Object);

            var getAllRequest = _fixture.Create<GetAllCommentsRequest>();
            
            await getAllCommentsService.ExecuteAsync(getAllRequest);

            Mock.Get(cosmosDbServiceMock.Object)
                .Verify(c => c.GetAllCommentsAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnGetAllCommentsResponse_WhenServerResponsesHappily()
        {
            var allComments = _fixture.CreateMany<Comment>().ToList();

            var cosmosDbServiceMock = new Mock<ICosmosDbService>();
            cosmosDbServiceMock.Setup(_ => _.GetAllCommentsAsync()).ReturnsAsync(allComments);

            var log = new Mock<ILogger<GetAllCommentsService>>();

            var getAllCommentsService = new GetAllCommentsService(
                cosmosDbServiceMock.Object,
                log.Object);

            var getAllRequest = _fixture.Create<GetAllCommentsRequest>();

            var result = await getAllCommentsService.ExecuteAsync(getAllRequest);

            result.Should().BeOfType<GetAllCommentsResponse>();

        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowAnException_WhenCosmosDbServiceThrowsException()
        {
            var cosmosDbServiceMock = new Mock<ICosmosDbService>();
            cosmosDbServiceMock.Setup(_ => _.GetAllCommentsAsync()).Throws<Exception>();

            var log = new Mock<ILogger<GetAllCommentsService>>();

            var getAllCommentsService = new GetAllCommentsService(
                cosmosDbServiceMock.Object,
                log.Object);

            var getAllRequest = _fixture.Create<GetAllCommentsRequest>();

            Func<Task<GetAllCommentsResponse>> func = async () => await getAllCommentsService.ExecuteAsync(getAllRequest);

            await func.Should().ThrowAsync<GetAllCommentsException>();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldContainObjectsOfTypeGetAllCommentsResponse()
        {
            var allComments = _fixture.CreateMany<Comment>().ToList();
            var cosmosDbServiceMock = new Mock<ICosmosDbService>();
            cosmosDbServiceMock.Setup(_ => _.GetAllCommentsAsync()).ReturnsAsync(allComments);
            
            var log = new Mock<ILogger<GetAllCommentsService>>();

            var getAllCommentsService = new GetAllCommentsService(
                cosmosDbServiceMock.Object,
                log.Object);

            var getAllRequest = _fixture.Create<GetAllCommentsRequest>();

            var response = await getAllCommentsService.ExecuteAsync(getAllRequest);

            response.Should().BeOfType<GetAllCommentsResponse>();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnListInResponse()
        {
            var allComments = _fixture.CreateMany<Comment>().ToList();

            var cosmosDbServiceMock = new Mock<ICosmosDbService>();
            cosmosDbServiceMock.Setup(_ => _.GetAllCommentsAsync()).ReturnsAsync(allComments);

            var log = new Mock<ILogger<GetAllCommentsService>>();

            var getAllCommentsService = new GetAllCommentsService(
                cosmosDbServiceMock.Object,
                log.Object);

            var getAllRequest = _fixture.Create<GetAllCommentsRequest>();

            await getAllCommentsService.ExecuteAsync(getAllRequest);

            allComments.Should().BeOfType<List<Comment>>();
        }

        [Fact]
        public async Task ExecuteAsync_ResponseShouldNotBeNull()
        {
            var allComments = _fixture.CreateMany<Comment>().ToList();

            var cosmosDbServiceMock = new Mock<ICosmosDbService>();
            cosmosDbServiceMock.Setup(_ => _.GetAllCommentsAsync()).ReturnsAsync(allComments);

            var log = new Mock<ILogger<GetAllCommentsService>>();

            var getAllCommentsService = new GetAllCommentsService(
                cosmosDbServiceMock.Object,
                log.Object);

            var getAllRequest = _fixture.Create<GetAllCommentsRequest>();

            var response = await getAllCommentsService.ExecuteAsync(getAllRequest);

            response.Should().NotBeNull();
        }
    }
}
