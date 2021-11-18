using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Kainos.Comments.Application.Model.Domain;
using Kainos.Comments.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Kainos.Comments.Functions.Functions
{
    class DeleteAllCommentsFunction
    {
        private readonly IExecutable<DeleteAllCommentsRequest, DeleteAllCommentsResponse> _repository;

        public DeleteAllCommentsFunction(IExecutable<DeleteAllCommentsRequest, DeleteAllCommentsResponse> repository)
        {
            _repository = repository;
        }

        [FunctionName(nameof(DeleteAllCommentsAsync))]
        public async Task<IActionResult> DeleteAllCommentsAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Delete), Route = Routes.DeleteAll)]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Deleting all comments");
            var deleteAllRequest = new DeleteAllCommentsRequest();

            try
            {
                await _repository.ExecuteAsync(deleteAllRequest);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new InternalServerErrorResult();
            }

            return new OkObjectResult(new DeleteAllCommentsResponse
            {
                Message = "List of all comments successfully deleted."
            });
        }

    }
}
