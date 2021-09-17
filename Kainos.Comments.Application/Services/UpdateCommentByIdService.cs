using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Kainos.Comments.Application.Cosmos;
using Kainos.Comments.Application.Model.Database;
using Kainos.Comments.Application.Model.Domain;
using Microsoft.Extensions.Logging;

namespace Kainos.Comments.Application.Services
{
    public class UpdateCommentByIdService : IExecutable<UpdateCommentRequest, UpdateCommentResponse>
    {
        private readonly ICosmosDbService _cosmosDb;
        private readonly ILogger<UpdateCommentByIdService> _log;

        public UpdateCommentByIdService(
            ICosmosDbService cosmosDb,
            ILogger<UpdateCommentByIdService> log)
        {
            _cosmosDb = cosmosDb;
            _log = log;
        }

        public async Task<UpdateCommentResponse> ExecuteAsync(UpdateCommentRequest request)
        {
            _log.LogInformation("Updating a comment");

            var updateComment = new Comment
            {
                Id = request.Id,
                Text = request.Text,
            };

            try
            {
                await _cosmosDb.UpdateCommentByIdAsync(request.Id, updateComment);
            }
            catch (Exception e)
            {
                _log.LogError(e.Message);
                throw;
            }

            return new UpdateCommentResponse
            {
                Id = updateComment.Id,
                Text = updateComment.Text,
                Message = "Comment successfully updated."
            };
        }
    }
}