using Azure.Storage.Queues;
using Kainos.Comments.Application.Model.Database;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Kainos.Comments.Application.Queue
{
    public class QueueService : IQueueService
    {
        private readonly QueueClient _queueClient;
        private readonly ILogger<QueueService> _log;

        public QueueService(
            QueueClient queueClient,
            ILogger<QueueService> log)
        {
            _queueClient = queueClient;
            _log = log;
        }

        public async Task ExecuteAsync(Comment comment)
        {
            var queueComment = JsonConvert.SerializeObject(comment);

            _log.LogInformation("Comment successfully passed to queue.");
            await _queueClient.SendMessageAsync(queueComment);
        }
    }
}

