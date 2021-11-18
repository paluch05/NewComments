using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using FluentValidation;
using Kainos.Comments.Application.Model.Domain;
using Kainos.Comments.Application.Services;
using Kainos.Comments.Functions.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kainos.Comments.Functions.Functions
{
    public class UpdateCommentByIdFunction
    {
        private readonly IExecutable<UpdateCommentRequest, UpdateCommentResponse> _repository;
        private readonly IValidator<UpdateCommentRequest> _updateValidator;

        public UpdateCommentByIdFunction(
            IExecutable<UpdateCommentRequest, UpdateCommentResponse> repository,
            IValidator<UpdateCommentRequest> updateValidator)
        {
            _repository = repository;
            _updateValidator = updateValidator;
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
                updateCommentRequest = JsonConvert.DeserializeObject<UpdateCommentRequest>(requestBody);
            }
            catch (JsonSerializationException jse)
            {
                log.LogError(jse, jse.Message);

                return new BadRequestObjectResult(new
                { reason = "Wrong Json format." });
            }
            updateCommentRequest.Id = id;

            try
            {
                var validationResult = await _updateValidator.ValidateAsync(updateCommentRequest);

                if (!validationResult.IsValid)
                {
                    return new BadRequestObjectResult(new {reason = validationResult.Errors});
                }

                var updateCommentResponse = await _repository.ExecuteAsync(updateCommentRequest);

                log.LogInformation("Comment successfully updated.");

                return new OkObjectResult(updateCommentResponse);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new InternalServerErrorResult();
            }
        }
    }
}
