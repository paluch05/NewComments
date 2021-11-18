using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kainos.Comments.Application.Cosmos;
using Kainos.Comments.Application.Exceptions;
using Kainos.Comments.Application.Model.Database;
using Kainos.Comments.Application.Model.Domain;
using Microsoft.Extensions.Logging;

namespace Kainos.Comments.Application.Services
{
    public class GetAllCommentsService : IExecutable<GetAllCommentsRequest, GetAllCommentsResponse>
    {
        private readonly ICosmosDbService _cosmosDb;
        private readonly ILogger<GetAllCommentsService> _log;

        public GetAllCommentsService(
            ICosmosDbService cosmosDb,
            ILogger<GetAllCommentsService> log)
        {
            _cosmosDb = cosmosDb;
            _log = log;
        }

        public async Task<GetAllCommentsResponse> ExecuteAsync(GetAllCommentsRequest request)
        {
            _log.LogInformation("Getting all comments");
            IEnumerable<Comment> comments;
            try
            {
                comments = await _cosmosDb.GetAllCommentsAsync();
            }
            catch (Exception e)
            {
                _log.LogError(e.Message);
                throw new GetAllCommentsException("Unable to get all comments.");
            }

            _log.LogInformation("List of all comments:");
            return new GetAllCommentsResponse
            {
                AllComments = comments.ToList()
            };
        }
    }
}
