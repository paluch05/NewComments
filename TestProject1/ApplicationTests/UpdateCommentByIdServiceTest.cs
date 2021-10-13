using System;
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
    public class UpdateCommentByIdServiceTest
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());

        [Fact]
        public async Task ExecuteAsync_ShouldUpdateComment()
        {
            var updateComment = _fixture.Create<Comment>();
            var cosmosDbServiceMock = new Mock<ICosmosDbService>();
            cosmosDbServiceMock.Setup(_ => _.UpdateCommentByIdAsync(updateComment.Id, updateComment)).Returns(Task.CompletedTask);

            var log = new Mock<ILogger<UpdateCommentByIdService>>();

            var update = new UpdateCommentByIdService(
                cosmosDbServiceMock.Object,
                log.Object);

            var updateRequest = _fixture.Create<UpdateCommentRequest>();

            await update.ExecuteAsync(updateRequest);

            Mock.Get(cosmosDbServiceMock.Object)
                .Verify(c => c.UpdateCommentByIdAsync(updateRequest.Id, It.IsAny<Comment>()), Times.Once);

        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnUpdateCommentResponse_WhenUpdateSucceeded()
        {
            var updateComment = _fixture.Create<Comment>();
            var cosmosDbServiceMock = new Mock<ICosmosDbService>();
            cosmosDbServiceMock.Setup(_ => _.UpdateCommentByIdAsync(updateComment.Id, updateComment)).Returns(Task.CompletedTask);

            var log = new Mock<ILogger<UpdateCommentByIdService>>();

            var update = new UpdateCommentByIdService(
                cosmosDbServiceMock.Object,
                log.Object);

            var updateRequest = _fixture.Create<UpdateCommentRequest>();

            var result = await update.ExecuteAsync(updateRequest);
            result.Should().BeOfType<UpdateCommentResponse>();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowAnException_WhenCosmosDbServiceThrowsException()
        {
            var cosmosDbServiceMock = new Mock<ICosmosDbService>();
            cosmosDbServiceMock
                .Setup(_ => _.UpdateCommentByIdAsync(It.IsAny<string>(), It.IsAny<Comment>()))
                .Throws<Exception>();

            var log = new Mock<ILogger<UpdateCommentByIdService>>();

            var update = new UpdateCommentByIdService(
                cosmosDbServiceMock.Object,
                log.Object);

            var updateRequest = _fixture.Create<UpdateCommentRequest>();
            Func<Task<UpdateCommentResponse>> func = async () => await update.ExecuteAsync(updateRequest);
            await func.Should().ThrowAsync<UpdateCommentByIdException>();
        }
    }
}
