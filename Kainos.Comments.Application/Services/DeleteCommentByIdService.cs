using System;
using System.Threading.Tasks;
using Kainos.Comments.Application.Cosmos;
using Kainos.Comments.Application.Model.Domain;
using Microsoft.Extensions.Logging;

namespace Kainos.Comments.Application.Services
{
    class DeleteCommentByIdService : IExecutable<DeleteCommentByIdRequest, DeleteCommentByIdResponse>
    {
        private readonly ICosmosDbService _cosmosDb;
        private readonly ILogger<DeleteCommentByIdService> _log;

        public DeleteCommentByIdService(
            ICosmosDbService cosmosDb,
            ILogger<DeleteCommentByIdService> log)
        {
            _cosmosDb = cosmosDb;
            _log = log;
        }

        public async Task<DeleteCommentByIdResponse> ExecuteAsync(DeleteCommentByIdRequest request)
        {
            _log.LogInformation("Deleting a comment");

            try
            {
                await _cosmosDb.DeleteCommentByIdAsync(request.Id);
            }
            catch (Exception e)
            {
                _log.LogError(e, e.Message);
                _log.LogError($"File with given id : {request.Id} was not found");

                return new DeleteCommentByIdResponse
                {
                    Message = $"File with given id: {request.Id} was not found"
                };
            }

            return new DeleteCommentByIdResponse
            {
                Message = $"Comment with given Id : {request.Id} successfully deleted."
            };
        }
    }
}
