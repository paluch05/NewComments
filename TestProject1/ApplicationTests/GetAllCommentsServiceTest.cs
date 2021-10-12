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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TestProject1.ApplicationTests
{
    public class GetAllCommentsServiceTest
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
        private readonly ICosmosDbService _cosmosDbServiceMock = Mock.Of<ICosmosDbService>();

        [Fact]
        public async Task ExecuteAsync_ShouldGetAllComments()
        {
            var allComments = _fixture.CreateMany<Comment>().ToList();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.GetAllCommentsAsync())
                .ReturnsAsync(allComments)
                .Verifiable();

            var log = new Mock<ILogger<GetAllCommentsService>>();

            var getAll = new GetAllCommentsService(
                _cosmosDbServiceMock,
                log.Object);

            var getAllRequest = _fixture.Create<GetAllCommentsRequest>();
            
            await getAll.ExecuteAsync(getAllRequest);

            Mock.Get(_cosmosDbServiceMock)
                .Verify(c => c.GetAllCommentsAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnGetAllCommentsResponse_WhenServerResponsesHappily()
        {
            var allComments = _fixture.CreateMany<Comment>().ToList();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.GetAllCommentsAsync())
                .ReturnsAsync(allComments)
                .Verifiable();

            var log = new Mock<ILogger<GetAllCommentsService>>();

            var getAll = new GetAllCommentsService(
                _cosmosDbServiceMock,
                log.Object);

            var getAllRequest = _fixture.Create<GetAllCommentsRequest>();

            var result = await getAll.ExecuteAsync(getAllRequest);

            result.Should().BeOfType<GetAllCommentsResponse>();

        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowAnException_WhenCosmosDbServiceThrowsException()
        {
            var cosmosDbServiceMock = new Mock<ICosmosDbService>();
            cosmosDbServiceMock.Setup(_ => _.GetAllCommentsAsync()).Throws<Exception>();
            var allComments = _fixture.CreateMany<Comment>().ToList();

            var log = new Mock<ILogger<GetAllCommentsService>>();

            var getAll = new GetAllCommentsService(
                cosmosDbServiceMock.Object,
                log.Object);

            var getAllRequest = _fixture.Create<GetAllCommentsRequest>();

            Func<Task<GetAllCommentsResponse>> func = async () => await getAll.ExecuteAsync(getAllRequest);

            await func.Should().ThrowAsync<GetAllCommentsException>();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldContainObjectsOfTypeGetAllCommentsResponse()
        {
            var allComments = _fixture.CreateMany<Comment>().ToList();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.GetAllCommentsAsync())
                .ReturnsAsync(allComments)
                .Verifiable();

            var log = new Mock<ILogger<GetAllCommentsService>>();

            var getAll = new GetAllCommentsService(
                _cosmosDbServiceMock,
                log.Object);

            var getAllRequest = _fixture.Create<GetAllCommentsRequest>();

            var response = await getAll.ExecuteAsync(getAllRequest);

            response.Should().BeOfType<GetAllCommentsResponse>();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnListInResponse()
        {
            var allComments = _fixture.CreateMany<Comment>().ToList();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.GetAllCommentsAsync())
                .ReturnsAsync(allComments)
                .Verifiable();

            var log = new Mock<ILogger<GetAllCommentsService>>();

            var getAll = new GetAllCommentsService(
                _cosmosDbServiceMock,
                log.Object);

            var getAllRequest = _fixture.Create<GetAllCommentsRequest>();

            await getAll.ExecuteAsync(getAllRequest);

            allComments.Should().BeOfType<List<Comment>>();
        }

        [Fact]
        public async Task ExecuteAsync_ResponseShouldNotBeNull()
        {
            var allComments = _fixture.CreateMany<Comment>().ToList();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.GetAllCommentsAsync())
                .ReturnsAsync(allComments)
                .Verifiable();

            var log = new Mock<ILogger<GetAllCommentsService>>();

            var getAll = new GetAllCommentsService(
                _cosmosDbServiceMock,
                log.Object);

            var getAllRequest = _fixture.Create<GetAllCommentsRequest>();

            var response = await getAll.ExecuteAsync(getAllRequest);

            response.Should().NotBeNull();
        }
    }
}
