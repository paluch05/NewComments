using System;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Storage.Queues;
using FluentValidation;
using Kainos.Comments.Application.Cosmos;
using Kainos.Comments.Application.Model;
using Kainos.Comments.Application.Model.Domain;
using Kainos.Comments.Application.Queue;
using Kainos.Comments.Application.Search;
using Kainos.Comments.Application.Services;
using Kainos.Comments.Functions.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace Kainos.Comments.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services, Configuration.Configuration cosmosDbConfiguration,
                                            Configuration.Configuration queueConfiguration,
                                            Configuration.Configuration searchConfiguration)
        {
            var endpoint = new Uri(searchConfiguration.SearchEndpoint);
            var adminKey = new AzureKeyCredential(searchConfiguration.AdminKey);

            services.AddSingleton<CosmosClient>(provider =>
            {
                var cosmosClientBuilder = new CosmosClientBuilder(cosmosDbConfiguration.CosmosDbConnectionString);
                return cosmosClientBuilder.Build();
            });

            services.AddSingleton<QueueClient>(new QueueClient(queueConfiguration.QueueConnectionString, queueConfiguration.QueueName, 
                new QueueClientOptions
                {
                    MessageEncoding = QueueMessageEncoding.Base64
                }));

            services.AddSingleton<SearchClient>(new SearchClient(endpoint,
                searchConfiguration.IndexName, adminKey));

            services.AddTransient<SearchIndexClient>(_ => new SearchIndexClient(endpoint, adminKey));

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
            services.Configure<Configuration.Configuration>(c =>
            {
                c.IndexName = searchConfiguration.IndexName;
                c.AdminKey = searchConfiguration.AdminKey;
                c.QueryKey = searchConfiguration.QueryKey;
                c.SearchEndpoint = searchConfiguration.SearchEndpoint;
            });
            services.AddTransient<IValidator<AddCommentRequest>, AddCommentRequestValidator>();
            services.AddTransient<IValidator<UpdateCommentRequest>, UpdateCommentRequestValidator>();
            services.AddTransient<IExecutable<DeleteAllCommentsRequest, DeleteAllCommentsResponse>, DeleteAllCommentsService>();
            services.AddTransient<ISearchService, SearchService>();

        }
    }
}

