using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Kainos.Comments.Application;
using Kainos.Comments.Application.Model.Database;
using Kainos.Comments.Functions.Exceptions;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kainos.Comments.Functions.Functions
{
    public class QueueFunction
    {
        private readonly ICosmosExecutable _cosmosExecutable;

        public QueueFunction(ICosmosExecutable cosmosExecutable)
        {
            _cosmosExecutable = cosmosExecutable;
        }

        [FunctionName(nameof(QueueTriggerFunction))]
        public async Task QueueTriggerFunction(
            [QueueTrigger("updatetriggerqueue")] Comment comment, ILogger log)
        {
            log.LogInformation("Got a comment from queue");
            if (comment.IsCensored == false)
            {
                log.LogInformation("Successfully censored comment");
                await _cosmosExecutable.ExecuteAsync(comment);
            }
            else
            {
                throw new CensorCommentException("Unable to censor comment.");
            }
        }
    }
}
