using System;
using System.Threading.Tasks;
using Kainos.Comments.Application.Cosmos;
using Kainos.Comments.Application.Model;
using Kainos.Comments.Application.Model.Database;
using Microsoft.Extensions.Logging;

namespace Kainos.Comments.Application.Services
{
    public class AddCommentService : IExecutable<AddCommentRequest, AddCommentResponse>
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly ILogger<AddCommentService> _log;

        public AddCommentService(
            ICosmosDbService cosmosDbService,
            ILogger<AddCommentService> log)
        {
            _cosmosDbService = cosmosDbService;
            _log = log;
        }

        public async Task<AddCommentResponse> ExecuteAsync(AddCommentRequest request)
        {
            _log.LogInformation("Creating a new comment");

            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Author = request.Author,
                Text = request.Text,
                CreationDate = DateTime.UtcNow,
                IsCensored = false
            };

            var addedComment = await _cosmosDbService.AddCommentAsync(comment);

            if (addedComment == null)
            {
                return null; // kolejna 500
            }

            _log.LogInformation("Comment successfully created.");

            return new AddCommentResponse
            {
                Id = addedComment.Id,
                Message = "Comment successfully created."
            };
        }
    }
}
