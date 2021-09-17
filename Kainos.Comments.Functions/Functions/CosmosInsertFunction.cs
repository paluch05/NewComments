using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Kainos.Comments.Application;
using Kainos.Comments.Application.Model.Database;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents;

namespace Kainos.Comments.Functions.Functions
{
    public class CosmosInsertFunction
    {
        private readonly ICosmosExecutable _cosmosExecutable;

        public CosmosInsertFunction(
            ICosmosExecutable cosmosExecutable)
        {
            _cosmosExecutable = cosmosExecutable;
        }

        [FunctionName("CosmosInsertFunction")]
        public async Task Run([CosmosDBTrigger(
                databaseName: "Comments",
                collectionName: "Comments",
                ConnectionStringSetting = "CosmosDbConnectionString",
                CreateLeaseCollectionIfNotExists = true)]
            IReadOnlyList<Document> documents, ILogger log)
        {
            //var updatedComments = documents.Select(d => new Comment
            //{
            //    Id = d.GetPropertyValue<string>("id"),
            //    Author = d.GetPropertyValue<string>("author"),
            //    Text = d.GetPropertyValue<string>("text"),
            //    CreationDate = d.GetPropertyValue<DateTime>("creationDate"),
            //    IsCensored = d.GetPropertyValue<bool>("isCensored")
            //}).Where(c => c.IsCensored == false);

            var updatedComments = documents.Select(d =>
            {
                var id = d.GetPropertyValue<string>("id");
                var author = d.GetPropertyValue<string>("author");
                var text = d.GetPropertyValue<string>("text");
                var creationDate = d.GetPropertyValue<DateTime>("creationDate");
                var isCensored = d.GetPropertyValue<bool>("isCensored");
                return new Comment
                {
                    Id = d.GetPropertyValue<string>("id"),
                    Author = d.GetPropertyValue<string>("author"),
                    Text = d.GetPropertyValue<string>("text"),
                    CreationDate = d.GetPropertyValue<DateTime>("creationDate"),
                    IsCensored = d.GetPropertyValue<bool>("isCensored")
                };
            }).Where(c => c.IsCensored == false);

            foreach (var comment in updatedComments)
            {
                await _cosmosExecutable.ExecuteAsync(comment);
            }

        }

}
}
