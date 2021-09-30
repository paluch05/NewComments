using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
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
    class AddCommentFunction
    {
        private readonly IExecutable<AddCommentRequest, AddCommentResponse> _repository;

        public AddCommentFunction(
            IExecutable<AddCommentRequest, AddCommentResponse> repository)
        {
            _repository = repository;
        }

        [FunctionName(nameof(AddCommentAsync))]
        public async Task<IActionResult> AddCommentAsync([HttpTrigger(AuthorizationLevel.Anonymous,
                nameof(HttpMethods.Post),
                Route = Routes.Add)]
            HttpRequest req, ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            AddCommentRequest addCommentRequest;

            try
            {
                addCommentRequest =
                     JsonConvert.DeserializeObject<AddCommentRequest>(
                         requestBody);
            }
            catch (Exception e)
            {
                log.LogError(e, e.Message);
                log.LogError("Your Json format is incorrect.");

                return new BadRequestObjectResult(new
                {
                    reason = e.Message
                });
            }

            var addCommentRequestValidator = new AddCommentRequestValidator();
            var validationResult = await addCommentRequestValidator.ValidateAsync(addCommentRequest);

            if (!validationResult.IsValid)
            {
                log.LogError(validationResult.ToString());

                return new BadRequestObjectResult(new { reason = validationResult.ToString() });
            }

            var addCommentResponse = await _repository.ExecuteAsync(addCommentRequest);

            if (addCommentResponse == null)
            {
                return new InternalServerErrorResult(); // 500, ale z wiad
            }

            return new OkObjectResult(addCommentResponse);
        }
    }
}
