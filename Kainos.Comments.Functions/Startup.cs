using System;
using Kainos.Comments.Application.Configuration;
using Kainos.Comments.Application.Extensions;
using Kainos.Comments.Functions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

[assembly:FunctionsStartup(typeof(Startup))]

namespace Kainos.Comments.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // JsonConvert.DefaultSettings = () =>
            //     new JsonSerializerSettings
            //     {
            //         Formatting = Formatting.Indented,
            //         ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //     };

            var cosmosDbConfiguration = GetCosmosDbConfiguration();
            var queueConfiguration = GetQueueConfiguration();

            builder.Services.Configure<Configuration>(configuration =>
            {
                configuration.CosmosDbConnectionString = cosmosDbConfiguration.CosmosDbConnectionString;
                configuration.DatabaseName = cosmosDbConfiguration.DatabaseName;
                configuration.CommentContainerName = cosmosDbConfiguration.CommentContainerName;
                configuration.BlackListContainerName = cosmosDbConfiguration.BlackListContainerName;
                configuration.QueueConnectionString = queueConfiguration.QueueConnectionString;
                configuration.QueueName = queueConfiguration.QueueName;
            });

            builder.Services.AddApplication(cosmosDbConfiguration, queueConfiguration);
        }


        private static Configuration GetCosmosDbConfiguration()
        {
            return new Configuration
            {
                CosmosDbConnectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString"),
                DatabaseName = Environment.GetEnvironmentVariable("DatabaseName"),
                CommentContainerName = Environment.GetEnvironmentVariable("CommentContainerName"),
                BlackListContainerName = Environment.GetEnvironmentVariable("BlackListContainerName")
            };
        }

        private static Configuration GetQueueConfiguration()
        {
            return new Configuration
            {
                QueueConnectionString = Environment.GetEnvironmentVariable("QueueConnectionString"),
                QueueName = Environment.GetEnvironmentVariable("QueueName")
            };
        }
    }
}
