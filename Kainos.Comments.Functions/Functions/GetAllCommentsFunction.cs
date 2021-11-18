using System;
using System.Threading.Tasks;
using System.Web.Http;
using Kainos.Comments.Application.Model.Domain;
using Kainos.Comments.Application.Services;
using Kainos.Comments.Functions.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Kainos.Comments.Functions.Functions
{
    public class GetAllCommentsFunction
    {
        private readonly IExecutable<GetAllCommentsRequest, GetAllCommentsResponse> _repository;

        public GetAllCommentsFunction(IExecutable<GetAllCommentsRequest, GetAllCommentsResponse> repository)
        {
            _repository = repository;
        }

        [FunctionName(nameof(GetAllCommentsAsync))]
        public async Task<IActionResult> GetAllCommentsAsync([HttpTrigger(AuthorizationLevel.Anonymous,
                nameof(HttpMethods.Get), Route = Routes.Get)]
            HttpRequest req, ILogger log)
        {
            GetAllCommentsResponse getComment;

            log.LogInformation("Getting all comments");

            try
            {
                getComment = await _repository.ExecuteAsync(new GetAllCommentsRequest());
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return new InternalServerErrorResult();
            }
            
            log.LogInformation("List of all comments: ");

            return new OkObjectResult(new { result = getComment });
        }
    }
}