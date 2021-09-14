﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kainos.Comments.Application.Cosmos;
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
            IEnumerable<Comment> comments;
            try
            {
                comments = await _cosmosDb.GetAllComments();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return new GetAllCommentsResponse
            {
                AllComments = comments.ToList()
            };
        }
    }
}