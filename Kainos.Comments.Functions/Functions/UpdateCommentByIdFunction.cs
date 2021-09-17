using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using Kainos.Comments.Application.Model.Domain;
using Kainos.Comments.Application.Services;
using Kainos.Comments.Functions.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kainos.Comments.Functions.Functions
{
    class UpdateCommentByIdFunction
    {
        private readonly IExecutable<UpdateCommentRequest, UpdateCommentResponse> _repository;

        public UpdateCommentByIdFunction(
            IExecutable<UpdateCommentRequest, UpdateCommentResponse> repository)
        {
            _repository = repository;
        }

        [FunctionName(nameof(UpdateCommentByIdAsync))]
        public async Task<IActionResult> UpdateCommentByIdAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethods.Put), Route = Routes.Update)]
            HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Updating comment by id");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UpdateCommentRequest updateCommentRequest;

            try
            {
                updateCommentRequest =
                    JsonConvert.DeserializeObject<UpdateCommentRequest>(
                        requestBody);
            }
            catch (Exception e)
            {
                log.LogError(e, e.Message);
                log.LogError("Your JSON format is incorrect");

                return new BadRequestObjectResult(new
                { reason = e.Message });
            }

            updateCommentRequest.Id = id;

            var updateCommentRequestValidator = new UpdateCommentRequestValidator();
            var validationResult = await updateCommentRequestValidator.ValidateAsync(updateCommentRequest);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            try
            {
                var updateCommentResponse = await _repository.ExecuteAsync(updateCommentRequest);

                log.LogInformation("Comment successfully updated.");

                return new OkObjectResult(updateCommentResponse);
            }
            catch (Exception e)
            {
                return new InternalServerErrorResult();
            }
        }
    }
}
