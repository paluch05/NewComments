using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Kainos.Comments.Application.Model.Database;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Kainos.Comments.Application.Cosmos
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _container;
        private readonly ILogger<CosmosDbService> _log;

        public CosmosDbService(
            Container container, 
            ILogger<CosmosDbService> log)
        {
            _container = container;
            _log = log;
        }

        public Task<IEnumerable<Comment>> GetAllComments()
        {
            var iterator = _container.GetItemLinqQueryable<Comment>(true);

            return Task.FromResult<IEnumerable<Comment>>(iterator.ToList());
        }

        public async Task<Comment> GetCommentByIdAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Comment>(id, new PartitionKey(id));
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
               var response = await _container.CreateItemAsync<Comment>(comment);
                return response.Resource;
            }
            catch (Exception e)
            {
                _log.LogError(e, e.Message);
                return null;
            }
        }

        public async Task UpdateCommentByIdAsync(string id, Comment comment) // ogarnac authora
        {
            try
            {
               var response = await _container.ReplaceItemAsync<Comment>(comment, id, new PartitionKey(id));
            }
            catch (Exception e)
            {
                _log.LogError(e.Message);
                throw;
            }
        }

        public async Task DeleteCommentByIdAsync(string id)
        {
            await _container.DeleteItemAsync<Comment>(id, new PartitionKey(id));
        }
    }
}
