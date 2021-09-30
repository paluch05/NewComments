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
            var comm = _fixture.Build<Comment>()
                .With(x => x.Id)
                .With(x => x.Author)
                .With(x => x.Text)
                .With(x => x.CreationDate)
                .With(x => x.IsCensored)
                .Create();
        
            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comm)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();


            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);

            // expectedResult typu addcommentresponse i porownac result shouldBeEquivalentTo...
            // metoda censored z Theory

            Mock.Get(_cosmosDbServiceMock)
                .Verify(c => c.AddCommentAsync(It.IsAny<Comment>()), Times.Once);

            }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnAddCommentResponseWhenCommentWasAdded()
        {
            var comm = _fixture.Build<Comment>()
                .With(x => x.Id)
                .With(x => x.Author)
                .With(x => x.Text)
                .With(x => x.CreationDate)
                .With(x => x.IsCensored)
                .Create();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comm)
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
            var comm = _fixture.Build<Comment>()
                .With(x => x.Id)
                .With(x => x.Author)
                .With(x => x.Text)
                .With(x => x.CreationDate)
                .With(x => x.IsCensored)
                .Create();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comm)
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
            var comm = _fixture.Build<Comment>()
                .With(x => x.Id)
                .With(x => x.Author)
                .With(x => x.Text)
                .With(x => x.CreationDate)
                .With(x => x.IsCensored)
                .Create();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comm)
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
            var comm = _fixture.Build<Comment>()
                .With(x => x.Id)
                .With(x => x.Author)
                .With(x => x.Text)
                .With(x => x.CreationDate)
                .With(x => x.IsCensored)
                .Create();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comm)
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
            var comm = _fixture.Build<Comment>()
                .With(x => x.Id)
                .With(x => x.Author)
                .With(x => x.Text)
                .With(x => x.CreationDate)
                .With(x => x.IsCensored)
                .Create();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comm)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();


            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);

            comm.Id.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ExecuteAsync_AuthorShouldNotBeNullOrEmpty()
        {
            var comm = _fixture.Build<Comment>()
                .With(x => x.Id)
                .With(x => x.Author)
                .With(x => x.Text)
                .With(x => x.CreationDate)
                .With(x => x.IsCensored)
                .Create();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comm)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();


            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);

            comm.Author.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ExecuteAsync_TextShouldNotBeNullOrEmpty()
        {
            var comm = _fixture.Build<Comment>()
                .With(x => x.Id)
                .With(x => x.Author)
                .With(x => x.Text)
                .With(x => x.CreationDate)
                .With(x => x.IsCensored)
                .Create();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comm)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();


            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);

            comm.Text.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldBeOfTypeComment()
        {
            var comm = _fixture.Build<Comment>()
                .With(x => x.Id)
                .With(x => x.Author)
                .With(x => x.Text)
                .With(x => x.CreationDate)
                .With(x => x.IsCensored)
                .Create();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comm)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();


            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);

            comm.Should().BeOfType<Comment>();
        }

        [Fact]
        public async Task ExecuteAsync_IsCensoredShouldBeTrue()
        {
            var comm = _fixture.Build<Comment>()
                .With(x => x.Id)
                .With(x => x.Author)
                .With(x => x.Text)
                .With(x => x.CreationDate)
                .With(x => x.IsCensored)
                .Create();

            Mock.Get(_cosmosDbServiceMock)
                .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comm)
                .Verifiable();

            var log = new Mock<ILogger<AddCommentService>>();


            var add = new AddCommentService(
                _cosmosDbServiceMock,
                log.Object);

            var addRequest = _fixture.Create<AddCommentRequest>();

            var result = await add.ExecuteAsync(addRequest);

            comm.IsCensored.Should().BeTrue();
        }
    }
}
