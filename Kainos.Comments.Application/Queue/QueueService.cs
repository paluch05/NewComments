using Azure.Storage.Queues;
using Kainos.Comments.Application.Model.Database;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Kainos.Comments.Application.Queue
{
    public class QueueService : IQueueService
    {
        private readonly Configuration.Configuration _queueConfiguration;
        private readonly ILogger<QueueService> _log;

        public QueueService(
            IOptions<Configuration.Configuration> queueConfiguration,
            ILogger<QueueService> log)
        {
            _queueConfiguration = queueConfiguration.Value;
            _log = log;
        }

        public async Task ExecuteAsync(Comment comment)
        {
            var queueConnectionString = _queueConfiguration.QueueConnectionString;
            var queueName = _queueConfiguration.QueueName;
            var queueComment = JsonConvert.SerializeObject(comment);

            var client = new QueueClient(queueConnectionString, queueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            await client.SendMessageAsync(queueComment);
        }
    }
}

