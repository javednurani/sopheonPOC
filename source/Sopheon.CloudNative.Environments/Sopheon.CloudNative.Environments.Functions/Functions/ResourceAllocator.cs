using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;

namespace Sopheon.CloudNative.Environments.Functions.Functions
{
   public class ResourceAllocator
   {
      private readonly IResourceAllocatorHelper _resourceAllocatorHelper;
      private readonly HttpResponseDataBuilder _responseBuilder;

      public ResourceAllocator(IResourceAllocatorHelper resourceAllocatorHelper, HttpResponseDataBuilder responseBuilder)
      {
         _resourceAllocatorHelper = resourceAllocatorHelper;
         _responseBuilder = responseBuilder;
      }

      [Function(nameof(ResourceAllocator))]
      [OpenApiOperation(operationId: nameof(ResourceAllocator),
         tags: new[] { "EnvironmentResourceBindings" },
         Summary = "Allocate a Resource for an Environment, and create a set of EnvironmentResourceBindings (satisfying all BusinessServiceDependencies) for that Environment & Resource",
         Description = "Allocate a Resource for an Environment, and create a set of EnvironmentResourceBindings (satisfying all BusinessServiceDependencies) for that Environment & Resource",
         Visibility = OpenApiVisibilityType.Important)]
      [OpenApiParameter(name: "environmentKey",
         Type = typeof(Guid),
         Required = true,
         Description = "The EnvironmentKey of the Environment",
         Summary = "The EnvironmentKey of the Environment")]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ResourceAllocatorResponseDto),
         Summary = StringConstants.RESPONSE_SUMMARY_201,
         Description = StringConstants.RESPONSE_DESCRIPTION_201)]
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",
            Route = "AllocateResourcesForEnvironment({environmentKey})")] HttpRequestData req,
            FunctionContext context, string environmentKey)
      {
         ILogger logger = context.GetLogger(nameof(ResourceAllocator));
         logger.LogInformation($"{nameof(ResourceAllocator)} Steel Thread");

         try
         {
            // TODO, consolidate duplication around 'valid Guid' between ResourceAllocator and DeleteEnvironment
            Guid parsedEnvironmentKey;
            bool validKey = Guid.TryParse(environmentKey, out parsedEnvironmentKey);
            if (!validKey || parsedEnvironmentKey == Guid.Empty)
            {
               ErrorDto error = new ErrorDto
               {
                  StatusCode = (int)HttpStatusCode.BadRequest,
                  Message = StringConstants.RESPONSE_REQUEST_ENVIRONMENTKEY_INVALID,
               };
               logger.LogInformation(StringConstants.RESPONSE_REQUEST_ENVIRONMENTKEY_INVALID);
               return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.BadRequest, error);
            }

            await _resourceAllocatorHelper.AllocateResourcesForEnvironment(parsedEnvironmentKey);
            return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.Created, new ResourceAllocatorResponseDto { Message = "steel thread"});
         }
         catch (Exception ex)
         {
            // TODO consolidate duplicated Exception handling between HttpTrigger Functions
            ErrorDto error = new ErrorDto
            {
               StatusCode = (int)HttpStatusCode.InternalServerError,
               Message = StringConstants.RESPONSE_GENERIC_ERROR,
            };
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.InternalServerError, error);
         }
      }
   }
}

