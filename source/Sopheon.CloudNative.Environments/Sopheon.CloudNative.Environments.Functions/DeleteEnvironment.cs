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
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;
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
      [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
         Summary = "204 No Content response",
         Description = "No Content, successful Delete")]
      [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound,
         Summary = "404 Not Found response",
         Description = "Not Found, Environment to be deleted was not found")]
      [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest,
         Summary = "400 Bad Request response",
         Description = "Bad Request")]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError,
         contentType: "application/json",
         bodyType: typeof(string),
         Summary = "500 Internal Server Error",
         Description = "Internal Server Error, 500 response with error message in response body")]

      public async Task<HttpResponseData> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "environments/{key}")] HttpRequestData req,
          FunctionContext context, string key)
      {
         var logger = context.GetLogger(nameof(DeleteEnvironment));

         try
         {
            Guid environmentKey;
            bool validKey = Guid.TryParse(key, out environmentKey);
            if (!validKey || environmentKey == Guid.Empty)
            {
               string keyNotGuidMessage = "The EnvironmentKey must be a valid Guid";
               logger.LogInformation(keyNotGuidMessage);
               return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.BadRequest, keyNotGuidMessage);
            }

            Environment environment = new Environment
            {
               EnvironmentKey = environmentKey
            };

            await _environmentRepository.DeleteEnvironment(environment);
            // TODO: soft delete, 202 Accepted vs 204 No Content
            return _responseBuilder.BuildWithoutBody(req, HttpStatusCode.NoContent);
         }
         catch (EntityNotFoundException ex)
         {
            logger.LogInformation(ex.Message);
            return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.NotFound, ex.Message);
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.InternalServerError, $"Something went wrong. Please try again later. {ex.Message}");
         }
      }
   }
}
