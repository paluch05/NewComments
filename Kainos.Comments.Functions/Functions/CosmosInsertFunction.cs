﻿using Kainos.Comments.Application.Model.Database;
using Kainos.Comments.Application.Queue;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kainos.Comments.Functions.Functions
{
    public class CosmosInsertFunction
    {
        private readonly IQueueService _queryService;

        public CosmosInsertFunction(
           IQueueService queryService)
        {
            _queryService = queryService;
        }

        [FunctionName("CosmosInsertFunction")]
        public async Task Run([CosmosDBTrigger(
                databaseName: "Comments",
                collectionName: "Comments",
                ConnectionStringSetting = "CosmosDbConnectionString",
                CreateLeaseCollectionIfNotExists = true)]
            IReadOnlyList<Document> documents, ILogger log)
        {
            var comments = documents.Select(d => JsonConvert.DeserializeObject<Comment>(d.ToString()));
            var updatedComments = comments.Where(c => c.IsCensored == false);

            foreach (var comment in updatedComments)
            {
                await _queryService.ExecuteAsync(comment); 
            }
        }
    }
}
