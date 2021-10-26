using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using FluentValidation;
using Kainos.Comments.Application.Model;
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
    public class AddCommentFunction
    {
        private readonly IExecutable<AddCommentRequest, AddCommentResponse> _repository;
        private readonly IValidator<AddCommentRequest> _validator;

        public AddCommentFunction(
            IExecutable<AddCommentRequest, AddCommentResponse> repository,
            IValidator<AddCommentRequest> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [FunctionName(nameof(AddCommentAsync))]
        public async Task<IActionResult> AddCommentAsync([HttpTrigger(AuthorizationLevel.Anonymous,
                nameof(HttpMethods.Post),
                Route = Routes.Add)]
            HttpRequest req, ILogger log)
        {
            log.LogInformation("Creating a comment");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            AddCommentRequest addCommentRequest;

            try
            {
                addCommentRequest = JsonConvert.DeserializeObject<AddCommentRequest>(requestBody);
            }
            catch (JsonSerializationException jse)
            {
                log.LogError(jse.Message);

                return new BadRequestObjectResult( new { reason = "Wrong Json format." });
            }

            try
            {
                var validationResult = await _validator.ValidateAsync(addCommentRequest);

                if (!validationResult.IsValid)
                {
                    log.LogError(validationResult.ToString());

                    return new BadRequestObjectResult(new {reason = validationResult.ToString()});
                }

                var addCommentResponse = await _repository.ExecuteAsync(addCommentRequest);

                return new OkObjectResult(addCommentResponse);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return new InternalServerErrorResult();
            }
        }
    }
}
