using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Kainos.Comments.Application.Configuration;
using Kainos.Comments.Application.Model.Database;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
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

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            var allComments = new List<Comment>();
            using var setIterator = _commentsContainer.GetItemLinqQueryable<Comment>().ToFeedIterator();
            while (setIterator.HasMoreResults)
            {
                foreach (var comment in await setIterator.ReadNextAsync())
                {
                    allComments.Add(comment);
                }
            }

            return allComments;
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
                _log.LogError(ce.Message);
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
                    CreationDate = commentResource.CreationDate,
                    IsCensored = comment.IsCensored
                };

                var response = await _commentsContainer.ReplaceItemAsync<Comment>(updatedComment, id, new PartitionKey(id));
            }
            catch (Exception e)
            {
                _log.LogError(e.Message);
                throw;
            }
        }

        public async Task<IEnumerable<BlackListItem>> GetAllBadWordsAsync()
        {
            var badWords = new List<BlackListItem>();

            using var setIterator = _blackListContainer.GetItemLinqQueryable<BlackListItem>().ToFeedIterator();
            
            while (setIterator.HasMoreResults)
            {
                badWords.AddRange(await setIterator.ReadNextAsync());
            }
            
            return badWords;
        }

        public async Task DeleteCommentByIdAsync(string id)
        {
            await _commentsContainer.DeleteItemAsync<Comment>(id, new PartitionKey(id));
        }
    }
}
