using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Kainos.Comments.Application.Cosmos;
using Kainos.Comments.Application.Model.Database;
using Kainos.Comments.Application.Model.Domain;
using Microsoft.Extensions.Logging;

namespace Kainos.Comments.Application.Search
{
    public class SearchService : ISearchService
    {
        private readonly SearchClient _searchClient;
        private readonly SearchIndexClient _searchIndex;
        private readonly ICosmosDbService _cosmosDbService;
        private readonly ILogger<SearchService> _log;

        public SearchService(SearchClient searchClient, 
            SearchIndexClient searchIndex,
            ICosmosDbService cosmosDbService,
            ILogger<SearchService> log)
        {
            _searchClient = searchClient;
            _searchIndex = searchIndex;
            _cosmosDbService = cosmosDbService;
            _log = log;
        }
        
        public async Task CreateIndexAsync()
        {
            var fields = new List<SearchField>();
        
            var idField = new SearchField("Id", SearchFieldDataType.String)
            {
                IsSearchable = false,
                IsKey = true
            };
            fields.Add(idField);
        
            var authorField = new SearchField("Author", SearchFieldDataType.String)
            {
                IsSearchable = true,
                IsHidden = false
            };
            fields.Add(authorField);
        
            var textField = new SearchField("Text", SearchFieldDataType.String)
            {
                IsSearchable = true,
                IsHidden = false
            };
            fields.Add(textField);

            try
            {
                _log.LogInformation("Index successfully created.");
                await _searchIndex.CreateOrUpdateIndexAsync(new SearchIndex(_searchClient.IndexName, fields));
            }
            catch (Exception e)
            {
                _log.LogError(e.Message);
            }
        }

        public async Task AddCommentsAsync()
        {
            _log.LogInformation("Adding comments to index.");
            var allComments = await _cosmosDbService.GetAllSearchCommentAsync(); // albo dodac wszystkie rzeczy w def indeksu albo stworzyc nowa klase do search
            
            var createArguments = allComments.Select(IndexDocumentsAction.Upload).ToArray(); // jak upsert
            var batch = IndexDocumentsBatch.Create(createArguments);

            try
            {
                _log.LogInformation("Comments successfully added to index");
               var result = await _searchClient.IndexDocumentsAsync(batch);
            }
            catch (Exception e)
            {
                _log.LogError("Unable to upsert comments");
                throw new Exception(e.Message);
            }
        }
    }
}
