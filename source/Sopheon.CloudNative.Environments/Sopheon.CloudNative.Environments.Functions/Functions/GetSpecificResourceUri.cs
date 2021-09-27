using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Domain.Queries;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class GetSpecificResourceUri
   {

      private readonly IEnvironmentQueries _environmentQueries;
      private IMapper _mapper;
      private readonly HttpResponseDataBuilder _responseBuilder;

      public GetSpecificResourceUri(IEnvironmentQueries environmentQueries, IMapper mapper, HttpResponseDataBuilder responseBuilder)
      {
         _environmentQueries = environmentQueries;
         _mapper = mapper;
         _responseBuilder = responseBuilder;
      }

      [Function(nameof(GetSpecificResourceUri))]
      [OpenApiOperation(operationId: nameof(GetSpecificResourceUri),
         tags: new[] { "Resources" },
         Summary = "",
         Description = "",
         Visibility = OpenApiVisibilityType.Important)]
      [OpenApiParameter(name: "environmentKey",
         Type = typeof(Guid),
         Required = true,
         Description = "The key of the Environment",
         Summary = "The key of the Environment")]
      [OpenApiParameter(name: "businessServiceName",
         Type = typeof(string),
         Required = true,
         Description = "The name of the BusinessService",
         Summary = "The name of the BusinessService")]
      [OpenApiParameter(name: "dependencyName",
         Type = typeof(string),
         Required = true,
         Description = "The name of the BusinessServiceDependency",
         Summary = "The name of the BusinessServiceDependency")]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ResourceUriDto),
         Summary = StringConstants.RESPONSE_SUMMARY_200,
         Description = StringConstants.RESPONSE_DESCRIPTION_200)]
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",
            Route = "GetEnvironmentResourceBindingUri({environmentKey}, {businessServiceName}, {dependencyName})")] HttpRequestData req,
          FunctionContext context, Guid environmentKey, string businessServiceName, string dependencyName)
      {
         var logger = context.GetLogger(nameof(GetSpecificResourceUri));
         try
         {
            // TODO, other validation eg minLength?
            if (Guid.Empty.Equals(environmentKey) || string.IsNullOrEmpty(businessServiceName) || string.IsNullOrEmpty(dependencyName))
            {
               ErrorDto exception = new ErrorDto
               {
                  StatusCode = (int)HttpStatusCode.BadRequest,
                  Message = StringConstants.RESPONSE_REQUEST_PATH_PARAMETER_INVALID,
               };
               logger.LogInformation(StringConstants.RESPONSE_REQUEST_PATH_PARAMETER_INVALID);
               return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.BadRequest, exception);
            }

            string resourceUri = await _environmentQueries.GetSpecificResourceUri(environmentKey, businessServiceName, dependencyName);

            ResourceUriDto dto = new ResourceUriDto
            {
               Uri = resourceUri
            };

            return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.OK, dto);
         }
         catch (EntityNotFoundException ex)
         {
            ErrorDto exception = new ErrorDto
            {
               StatusCode = (int)HttpStatusCode.NotFound,
               Message = ex.Message,
            };
            logger.LogInformation(ex.Message);
            return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.NotFound, exception);
         }
         catch (Exception ex)
         {
            ErrorDto exception = new ErrorDto
            {
               StatusCode = (int)HttpStatusCode.InternalServerError,
               Message = StringConstants.RESPONSE_GENERIC_ERROR,
            };
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.InternalServerError, StringConstants.RESPONSE_GENERIC_ERROR);
         }
      }
   }
}

