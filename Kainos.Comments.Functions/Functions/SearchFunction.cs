using System;
using System.Net.Http;
using System.Threading.Tasks;
using Azure;
using Kainos.Comments.Application.Model.Domain;
using Kainos.Comments.Application.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Kainos.Comments.Functions.Functions
{
    public class SearchFunction
    {
        private readonly ISearchService _searchService;

        public SearchFunction(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [FunctionName(nameof(SearchFunctionAsync))]
        public async Task<IActionResult> SearchFunctionAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = Routes.Search)]
            HttpRequest req,
            ILogger log, string value)
        {
            log.LogInformation("Searching for comment");

            var queryKey = Environment.GetEnvironmentVariable("QueryKey");
            var endpointValue = Environment.GetEnvironmentVariable("SearchEndpoint");
            var endpoint = new Uri(endpointValue);
            AzureKeyCredential credential = new AzureKeyCredential(queryKey);
            
            await _searchService.CreateIndexAsync();
            await _searchService.AddCommentsAsync();

            var client = new Azure.Search.Documents.Indexes.SearchIndexClient(endpoint, credential);
            var searchClient = client.GetSearchClient("comments");
            var searchResults = searchClient.Search<SearchComment>(value).Value;
            foreach (Azure.Search.Documents.Models.SearchResult<SearchComment> result in searchResults.GetResults())
            {
                var searchResult = new OkObjectResult($"Score:{result.Score}," +  
                                                   $"Id:{result.Document.Id}," +
                                                   $"Author:{result.Document.Author}," +
                                                   $"Text:{result.Document.Text},"
                );
            }

            log.LogInformation("Searching successfully ended.");
            return new OkObjectResult(new {result = searchResults.GetResults()});
        }
    }
}
