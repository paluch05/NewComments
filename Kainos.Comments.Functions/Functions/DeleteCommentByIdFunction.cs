using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kainos.Comments.Application.Services;
using Kainos.Comments.Application.Model.Domain;
using Kainos.Comments.Application.Services;
using Kainos.Comments.Functions.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Kainos.Comments.Functions.Functions
{
    class DeleteCommentByIdFunction
    {
        private readonly IExecutable<DeleteCommentByIdRequest, DeleteCommentByIdResponse> _repository;

        public DeleteCommentByIdFunction(IExecutable<DeleteCommentByIdRequest, DeleteCommentByIdResponse> repository)
        {
            _repository = repository;
        }

        [FunctionName(nameof(DeleteCommentByIdAsync))]
        public async Task<IActionResult> DeleteCommentByIdAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Delete), Route = Routes.Delete)]
            HttpRequest req,
            ILogger log, string id)
        {
            var deleteRequest = new DeleteCommentByIdRequest
            {
                Id = id
            };

            var deleteCommentRequestValidator = new DeleteCommentRequestValidator();
            var validationResult = await deleteCommentRequestValidator.ValidateAsync(deleteRequest);

            if (!validationResult.IsValid)
            {
                log.LogError(validationResult.ToString());
                return new BadRequestObjectResult(new {reason = validationResult.ToString()});
            }

            await _repository.ExecuteAsync(deleteRequest);
            return new OkObjectResult(new DeleteCommentByIdResponse
            {
                Message = "Comment successfully deleted."
            });
        }

        // getById czy usuwane id w ogole istnieje w db

    }
}
