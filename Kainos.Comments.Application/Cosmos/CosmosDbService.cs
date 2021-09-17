using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Kainos.Comments.Application.Configuration;
using Kainos.Comments.Application.Model.Database;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kainos.Comments.Application.Cosmos
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _commentsContainer;
        private readonly Container _blackListContainer;
        private readonly ILogger<CosmosDbService> _log;

        public CosmosDbService(
            CosmosClient cosmosClient, 
            IOptions<CosmosDbConfiguration> cosmosDbConfiguration,
            ILogger<CosmosDbService> log)
        {
            _commentsContainer = cosmosClient.GetContainer(cosmosDbConfiguration.Value.DatabaseName, cosmosDbConfiguration.Value.CommentContainerName);
            _blackListContainer = cosmosClient.GetContainer(cosmosDbConfiguration.Value.DatabaseName, cosmosDbConfiguration.Value.BlackListContainerName);
            _log = log;
        }

        public Task<IEnumerable<Comment>> GetAllComments()
        {
            var iterator = _commentsContainer.GetItemLinqQueryable<Comment>(true);

            return Task.FromResult<IEnumerable<Comment>>(iterator.ToList());
        }

        public async Task<Comment> GetCommentByIdAsync(string id)
        {
            try
            {
                var response = await _commentsContainer.ReadItemAsync<Comment>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ce) when (ce.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            try
            {
               var response = await _commentsContainer.CreateItemAsync<Comment>(comment);
                return response.Resource;
            }
            catch (Exception e)
            {
                _log.LogError(e, e.Message);
                return null;
            }
        }

        public async Task UpdateCommentByIdAsync(string id, Comment comment) 
        {
            try
            {
                var readComment = await _commentsContainer.ReadItemAsync<Comment>(id, new PartitionKey(id));
                var commentResource = readComment.Resource;
                
                var updatedComment = new Comment
                {
                    Id = id,
                    Author = commentResource.Author,
                    Text = comment.Text,
                    CreationDate = DateTime.UtcNow
                };

                var response = await _commentsContainer.ReplaceItemAsync<Comment>(updatedComment, id, new PartitionKey(id));
            }
            catch (Exception e)
            {
                _log.LogError(e.Message);
                throw;
            }
        }

        public async Task DeleteCommentByIdAsync(string id)
        {
            await _commentsContainer.DeleteItemAsync<Comment>(id, new PartitionKey(id));
        }

        public Task<IEnumerable<BlackListItem>> GetAllBadWords()
        {
            var iterator = _blackListContainer.GetItemLinqQueryable<BlackListItem>(true);

            return Task.FromResult<IEnumerable<BlackListItem>>(iterator.ToList());
        }
    }
}
