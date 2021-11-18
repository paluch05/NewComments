using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Kainos.Comments.Application.Cosmos;
using Kainos.Comments.Application.Model.Domain;
using Microsoft.Extensions.Logging;

namespace Kainos.Comments.Application.Services
{
    public class DeleteAllCommentsService : IExecutable<DeleteAllCommentsRequest, DeleteAllCommentsResponse>
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly ILogger<DeleteAllCommentsService> _log;

        public DeleteAllCommentsService(ICosmosDbService cosmosDbService, ILogger<DeleteAllCommentsService> log)
        {
            _cosmosDbService = cosmosDbService;
            _log = log;
        }

        public async Task<DeleteAllCommentsResponse> ExecuteAsync(DeleteAllCommentsRequest request)
        {
            _log.LogInformation("Deleting all comments");

            try
            {
                await _cosmosDbService.DeleteAllComments();
            }
            catch (Exception e)
            {
                _log.LogError(e.Message);
                _log.LogError("Unable to delete all comments.");
                return new DeleteAllCommentsResponse
                {
                    Message = e.Message
                };
            }

            return new DeleteAllCommentsResponse
            {
                Message = "All Comments successfully deleted."
            };
        }
    }
}
