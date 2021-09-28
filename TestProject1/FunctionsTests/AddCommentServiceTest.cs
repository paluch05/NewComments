using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Kainos.Comments.Application.Cosmos;
using Kainos.Comments.Application.Model.Database;
using Kainos.Comments.Application.Services;
using Moq;
using Xunit;

namespace TestProject1.FunctionsTests
{
    class AddCommentServiceTest
    {
        // private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
        //
        // [Fact]
        // public async Task ExecuteAsync JakakolwiekNazwaTestu()
        // {
        //
        //     var cosmosDbService = Mock.Of<ICosmosDbService>();
        //
        //     var comm = _fixture.Build<Comment>()
        //         .With(x => x.Id)
        //         .With(x => x.Author)
        //         .With(x => x.Text)
        //         .With(x => x.CreationDate)
        //         .With(x => x.IsCensored)
        //         .Create();
        //
        //     Mock.Get(cosmosDbService)
        //         .Setup(mock => mock.AddCommentAsync(It.IsAny<Comment>()))
        //         .ReturnsAsync(comm)
        //         .Verifiable();
        //
        //     var add = new AddCommentService()


        }




    }
}
