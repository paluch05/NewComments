using Azure.Storage.Queues;
using Kainos.Comments.Application.Cosmos;
using Kainos.Comments.Application.Model;
using Kainos.Comments.Application.Model.Domain;
using Kainos.Comments.Application.Queue;
using Kainos.Comments.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace Kainos.Comments.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services, Configuration.Configuration cosmosDbConfiguration,
                                            Configuration.Configuration queueConfiguration)
        {
            services.AddSingleton<CosmosClient>(provider =>
            {
                var cosmosClientBuilder = new CosmosClientBuilder(cosmosDbConfiguration.CosmosDbConnectionString);
                return cosmosClientBuilder.Build();
            });

            services.AddSingleton<QueueClient>(new QueueClient(queueConfiguration.QueueConnectionString, queueConfiguration.QueueName));

            services.AddTransient<IExecutable<AddCommentRequest, AddCommentResponse>, AddCommentService>();
            services.AddTransient<IExecutable<DeleteCommentByIdRequest, DeleteCommentByIdResponse>, DeleteCommentByIdService>();
            services.AddTransient<IExecutable<GetAllCommentsRequest, GetAllCommentsResponse>, GetAllCommentsService>();
            services.AddTransient<IExecutable<UpdateCommentRequest, UpdateCommentResponse>, UpdateCommentByIdService>();
            services.AddTransient<ICosmosExecutable, CensorCommentService>();
            services.AddTransient<ICosmosDbService, CosmosDbService>();
            services.AddTransient<IQueueService, QueueService>();
            services.Configure<Configuration.Configuration>(c =>
            {
                c.QueueConnectionString = queueConfiguration.QueueConnectionString;
                c.QueueName = queueConfiguration.QueueName;
            });

        }
    }
}

