using System;
using Kainos.Comments.Application.Configuration;
using Kainos.Comments.Application.Extensions;
using Kainos.Comments.Functions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly:FunctionsStartup(typeof(Startup))]

namespace Kainos.Comments.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var cosmosDbConfiguration = GetCosmosDbConfiguration();

            builder.Services.AddApplication(cosmosDbConfiguration);
        }

        private static CosmosDbConfiguration GetCosmosDbConfiguration()
        {
            return new CosmosDbConfiguration
            {
                CosmosDbConnectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString"),
                DatabaseName = Environment.GetEnvironmentVariable("DatabaseName"),
                ContainerName = Environment.GetEnvironmentVariable("ContainerName")
            };
        }
    }
}

// jak pojawiaja sie local.settings z powyzszym, jak environment sie laczy z json, jak zmienna z pliku zamienia sie w zmienna srodowiskowa
// co to zmienna srodowiskowa 



// cosmos trigger, queue trigger z cenzura slow. baza z brzydkimi slowkami i zamieniane sa na gwiazdki, jedna kolekcja z comment, druga black list.
// azure storage queue, http trigger, blacklist ma miec recordy kazde slowo osobno