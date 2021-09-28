using Kainos.Comments.Application.Configuration;
using Kainos.Comments.Application.Cosmos;
using Kainos.Comments.Application.Model;
using Kainos.Comments.Application.Model.Domain;
using Kainos.Comments.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace Kainos.Comments.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services, CosmosDbConfiguration cosmosDbConfiguration)
        {

            // services.AddTransient<Container>(provider =>
            // {
            //     var cosmosClientBuilder = new CosmosClientBuilder(cosmosDbConfiguration.CosmosDbConnectionString);
            //     var cosmosClient = cosmosClientBuilder.Build();
            //
            //     return cosmosClient.GetContainer(cosmosDbConfiguration.DatabaseName, cosmosDbConfiguration.CommentContainerName);
            // });

            services.AddTransient<CosmosClient>(provider =>
            {
                var cosmosClientBuilder = new CosmosClientBuilder(cosmosDbConfiguration.CosmosDbConnectionString);
                return cosmosClientBuilder.Build();
            });
           
            services.AddTransient<IExecutable<AddCommentRequest, AddCommentResponse>, AddCommentService>();
            services.AddTransient<IExecutable<DeleteCommentByIdRequest, DeleteCommentByIdResponse>, DeleteCommentByIdService>();
            services.AddTransient<IExecutable<GetAllCommentsRequest, GetAllCommentsResponse>, GetAllCommentsService>();
            services.AddTransient<IExecutable<UpdateCommentRequest, UpdateCommentResponse>, UpdateCommentByIdService>();
            services.AddTransient<ICosmosExecutable, CensorCommentService>();
            services.AddTransient<ICosmosDbService, CosmosDbService>();

        }
    }
}

// extensions doczytac
// dlaczego this jest tutaj, dlaczego jest static i jak dziala
// SCOPE


// testy, kolejka comments, jak tworze, zwracam obiekt niepotrzebnie
