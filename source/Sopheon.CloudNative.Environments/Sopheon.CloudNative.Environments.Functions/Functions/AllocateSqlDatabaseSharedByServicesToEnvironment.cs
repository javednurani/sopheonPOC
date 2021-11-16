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
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;

namespace Sopheon.CloudNative.Environments.Functions.Functions
{
   public class AllocateSqlDatabaseSharedByServicesToEnvironment
   {
      private readonly IAllocateSqlDatabaseSharedByServicesToEnvironmentHelper _resourceAllocationHelper;
      private readonly HttpResponseDataBuilder _responseBuilder;

      public AllocateSqlDatabaseSharedByServicesToEnvironment(IAllocateSqlDatabaseSharedByServicesToEnvironmentHelper resourceAllocationHelper, HttpResponseDataBuilder responseBuilder)
      {
         _resourceAllocationHelper = resourceAllocationHelper;
         _responseBuilder = responseBuilder;
      }

      [Function(nameof(AllocateSqlDatabaseSharedByServicesToEnvironment))]
      [OpenApiOperation(operationId: nameof(AllocateSqlDatabaseSharedByServicesToEnvironment),
         tags: new[] { "EnvironmentResourceBindings" },
         Summary = "Allocate a SQL Database resource, which can be shared by multiple services, to an Environment",
         Description = "Allocate a SQL Database resource, which can be shared by multiple services, to an Environment",
         Visibility = OpenApiVisibilityType.Important)]
      [OpenApiParameter(name: "environmentKey",
         Type = typeof(Guid),
         Required = true,
         Description = "The EnvironmentKey of the Environment",
         Summary = "The EnvironmentKey of the Environment")]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ResourceAllocationResponseDto),
         Summary = StringConstants.RESPONSE_SUMMARY_201,
         Description = StringConstants.RESPONSE_DESCRIPTION_201)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ErrorDto),
         Summary = StringConstants.RESPONSE_SUMMARY_400,
         Description = StringConstants.RESPONSE_DESCRIPTION_400)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ErrorDto),
         Summary = StringConstants.RESPONSE_SUMMARY_404,
         Description = StringConstants.RESPONSE_DESCRIPTION_404)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ErrorDto),
         Summary = StringConstants.RESPONSE_SUMMARY_500,
         Description = StringConstants.RESPONSE_DESCRIPTION_500)]
      public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",
            Route = "AllocateSqlDatabaseSharedByServicesToEnvironment({environmentKey})")] HttpRequestData req,
            FunctionContext context, Guid environmentKey)
      {
         ILogger logger = context.GetLogger(nameof(AllocateSqlDatabaseSharedByServicesToEnvironment));
         logger.LogInformation($"Executing Function {nameof(AllocateSqlDatabaseSharedByServicesToEnvironment)}");

         try
         {
            if (environmentKey.Equals(Guid.Empty))
            {
               logger.LogInformation(StringConstants.RESPONSE_REQUEST_ENVIRONMENTKEY_INVALID);
               return await _responseBuilder.BuildWithErrorBodyAsync(req, HttpStatusCode.BadRequest, StringConstants.RESPONSE_REQUEST_ENVIRONMENTKEY_INVALID);
            }

            string subscriptionId = Environment.GetEnvironmentVariable("AzSubscriptionId");
            string resourceGroupName = Environment.GetEnvironmentVariable("AzResourceGroupName");
            string sqlServerName = Environment.GetEnvironmentVariable("AzSqlServerName");

            await _resourceAllocationHelper.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(environmentKey, subscriptionId, resourceGroupName, sqlServerName);
            return await _responseBuilder.BuildWithJsonBodyAsync(req, HttpStatusCode.Created, new ResourceAllocationResponseDto());
         }
         catch (EntityNotFoundException ex)
         {
            logger.LogError(ex.Message);
            return await _responseBuilder.BuildWithErrorBodyAsync(req, HttpStatusCode.NotFound, ex.Message);
         }
         catch (Exception ex)
         {
            logger.LogError($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithErrorBodyAsync(req, HttpStatusCode.InternalServerError, StringConstants.RESPONSE_GENERIC_ERROR);
         }
      }
   }
}

