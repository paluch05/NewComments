using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Kainos.Comments.Application.Cosmos;
using Kainos.Comments.Application.Model.Database;
using Kainos.Comments.Application.Services;
using Kainos.Comments.Functions.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TestProject1.ApplicationTests
{
    public class CensorCommentServiceTest
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());

        [Theory]
        [InlineData("kotek")]
        [InlineData("KOTEK")]
        [InlineData("szarfa")]
        [InlineData("SZARFA")]
        [InlineData("kotekSZARFA")]
        [InlineData("<kotek>")]
        [InlineData("<KOTEK>")]
        public async Task ExecuteAsync_ShouldCensorComment(string badWord)
        {
            var badComment = _fixture.Build<Comment>()
                .With(x => x.Id)
                .With(x => x.Author)
                .With(x => x.Text, badWord)
                .With(x => x.CreationDate)
                .With(x => x.IsCensored, false)
                .Create();
            var blackList = _fixture.Create<List<BlackListItem>>();
            var cosmosServiceMock = new Mock<ICosmosDbService>();
            cosmosServiceMock.Setup(_ => _.GetAllBadWordsAsync()).ReturnsAsync(blackList);
        
            var log = new Mock<ILogger<CensorCommentService>>();
        
            var getAllBadWords = new CensorCommentService(
                cosmosServiceMock.Object,
                log.Object);
        
            await getAllBadWords.ExecuteAsync(badComment);
        
            Mock.Get(cosmosServiceMock.Object)
                .Verify(b => b.GetAllBadWordsAsync());
        }

        [Theory]
        [InlineData("kotek")]
        [InlineData("KOTEK")]
        [InlineData("szarfa")]
        [InlineData("SZARFA")]
        [InlineData("kotekSZARFA")]
        [InlineData("<kotek>")]
        [InlineData("<KOTEK>")]
        public async Task ExecuteAsync_ShouldThrowAnException_WhenCosmosDbServiceThrowsException(string badWord)
        {
            var badComment = _fixture.Build<Comment>()
                .With(x => x.Id)
                .With(x => x.Author)
                .With(x => x.Text, badWord)
                .With(x => x.CreationDate)
                .With(x => x.IsCensored, false)
                .Create();
            var blackList = _fixture.Create<List<BlackListItem>>();
            var cosmosServiceMock = new Mock<ICosmosDbService>();
            cosmosServiceMock.Setup(_ => _.GetAllBadWordsAsync()).ReturnsAsync(blackList);

            var log = new Mock<ILogger<CensorCommentService>>();

            var getAllBadWords = new CensorCommentService(
                cosmosServiceMock.Object,
                log.Object);

            Func<Task> func = async () => await getAllBadWords.ExecuteAsync(null);

            await func.Should().ThrowAsync<Exception>();
        }
    }
}
