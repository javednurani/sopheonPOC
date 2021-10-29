using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;
using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class DeleteEnvironment
   {
      private readonly IEnvironmentRepository _environmentRepository;
      private readonly HttpResponseDataBuilder _responseBuilder;

      public DeleteEnvironment(IEnvironmentRepository environmentRepository, HttpResponseDataBuilder responseBuilder)
      {
         _environmentRepository = environmentRepository;
         _responseBuilder = responseBuilder;
      }

      [Function(nameof(DeleteEnvironment))]
      [OpenApiOperation(operationId: nameof(DeleteEnvironment),
         tags: new[] { "Environments" },
         Summary = "Delete an Environment",
         Description = "Delete an Environment by EnvironmentKey",
         Visibility = OpenApiVisibilityType.Important)]
      [OpenApiParameter(name: "environmentKey",
         Type = typeof(Guid),
         Required = true,
         Description = "The key of the Environment to delete.",
         Summary = "The key of the Environment to delete.")]
      [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
         Summary = StringConstants.RESPONSE_SUMMARY_204,
         Description = StringConstants.RESPONSE_DESCRIPTION_204)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ErrorDto),
         Summary = StringConstants.RESPONSE_SUMMARY_404,
         Description = StringConstants.RESPONSE_DESCRIPTION_404)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ErrorDto),
         Summary = StringConstants.RESPONSE_SUMMARY_400,
         Description = StringConstants.RESPONSE_DESCRIPTION_400)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ErrorDto),
         Summary = StringConstants.RESPONSE_SUMMARY_500,
         Description = StringConstants.RESPONSE_DESCRIPTION_500)]

      public async Task<HttpResponseData> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "environments/{environmentKey}")] HttpRequestData req,
          FunctionContext context, Guid environmentKey)
      {
         ILogger logger = context.GetLogger(nameof(DeleteEnvironment));

         try
         {
            if (environmentKey == Guid.Empty)
            {
               logger.LogInformation(StringConstants.RESPONSE_REQUEST_ENVIRONMENTKEY_INVALID);
               return await _responseBuilder.BuildWithErrorBodyAsync(req, new ErrorDto((int)(int)HttpStatusCode.BadRequest, StringConstants.RESPONSE_REQUEST_ENVIRONMENTKEY_INVALID));
            }

            await _environmentRepository.DeleteEnvironment(environmentKey);
            // TODO: soft delete, 202 Accepted vs 204 No Content
            return _responseBuilder.BuildWithoutBody(req, HttpStatusCode.NoContent);
         }
         catch (EntityNotFoundException ex)
         {
            logger.LogInformation(ex.Message);
            return await _responseBuilder.BuildWithErrorBodyAsync(req, new ErrorDto((int)(int)HttpStatusCode.NotFound, ex.Message));
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithErrorBodyAsync(req, new ErrorDto((int)(int)HttpStatusCode.InternalServerError, StringConstants.RESPONSE_GENERIC_ERROR));
         }
      }
   }
}
