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
            JsonConvert.DefaultSettings = () =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

            var cosmosDbConfiguration = GetCosmosDbConfiguration();

            builder.Services.Configure<CosmosDbConfiguration>(configuration =>
            {
                configuration.CosmosDbConnectionString = cosmosDbConfiguration.CosmosDbConnectionString;
                configuration.DatabaseName = cosmosDbConfiguration.DatabaseName;
                configuration.CommentContainerName = cosmosDbConfiguration.CommentContainerName;
                configuration.BlackListContainerName = cosmosDbConfiguration.BlackListContainerName;
            });

            builder.Services.AddApplication(cosmosDbConfiguration);
        }

        private static CosmosDbConfiguration GetCosmosDbConfiguration()
        {
            return new CosmosDbConfiguration
            {
                CosmosDbConnectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString"),
                DatabaseName = Environment.GetEnvironmentVariable("DatabaseName"),
                CommentContainerName = Environment.GetEnvironmentVariable("CommentContainerName"),
                BlackListContainerName = Environment.GetEnvironmentVariable("BlackListContainerName")
            };
        }
    }
}


// cosmos trigger, queue trigger z cenzura slow. baza z brzydkimi slowkami i zamieniane sa na gwiazdki, jedna kolekcja z comment, druga black list.
// azure storage queue, http trigger, blacklist ma miec recordy kazde slowo osobno