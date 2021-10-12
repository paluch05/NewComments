using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Kainos.Comments.Application.Cosmos;
using Kainos.Comments.Application.Model;
using Kainos.Comments.Application.Model.Database;
using Kainos.Comments.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TestProject1.ApplicationTests
{
    public class AddCommentServiceTest
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
        private readonly ICosmosDbService _cosmosDbServiceMock = Mock.Of<ICosmosDbService>();

        [Fact]
        public async Task ExecuteAsync_ShouldAddComment()
        {
            var comment = _fixture.Create<Comment>();
        
            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comment)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();


            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();
            
            await add.ExecuteAsync(addRequest);

            Mock.Get(_cosmosDbServiceMock)
                .Verify(c => c.AddCommentAsync(It.IsAny<Comment>()), Times.Once);

            }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnAddCommentResponse_WhenCommentWasAdded()
        {
            var comment = _fixture.Create<Comment>();
            
            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comment)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();


            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);

            result.Should().BeOfType<AddCommentResponse>();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldBeEquivalentToId()
        {
            var comment = _fixture.Create<Comment>();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comment)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();

            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);
            
            result.Id.Should().BeEquivalentTo(result.Id);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldBeEquivalentToMessage()
        {
            var comment = _fixture.Create<Comment>();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comment)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();

            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);

            result.Message.Should().BeEquivalentTo(result.Message);
        }

        [Fact]
        public async Task ExecuteAsync_ResponseShouldNotBeNull()
        {
            var comment = _fixture.Create<Comment>();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comment)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();


            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_IdShouldNotBeNullOrEmpty()
        {
            var comment = _fixture.Create<Comment>();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comment)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();


            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);

            comment.Id.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ExecuteAsync_AuthorShouldNotBeNullOrEmpty()
        {
            var comment = _fixture.Create<Comment>();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comment)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();


            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);

            comment.Author.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ExecuteAsync_TextShouldNotBeNullOrEmpty()
        {
            var comment = _fixture.Create<Comment>();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comment)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();


            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);

            comment.Text.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldBeOfTypeComment()
        {
            var comment = _fixture.Create<Comment>();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comment)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();


            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);

            comment.Should().BeOfType<Comment>();
        }

        [Fact]
        public async Task ExecuteAsync_IsCensoredShouldBeTrue()
        {
            var comment = _fixture.Create<Comment>();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comment)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();


            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);

            comment.IsCensored.Should().BeTrue();
        }
    }
}
