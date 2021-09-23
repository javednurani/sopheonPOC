using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Domain.Queries;
using Sopheon.CloudNative.Environments.Functions.Helpers;

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
         tags: new[] { "Environments", "Resources", "BusinessServiceDependencies", "EnvironmentResourceBindings" },
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
         contentType: StringConstants.CONTENT_TYPE_TEXT_PLAIN,
         bodyType: typeof(string),
         Summary = StringConstants.RESPONSE_SUMMARY_200,
         Description = StringConstants.RESPONSE_DESCRIPTION_200)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound,
         contentType: StringConstants.CONTENT_TYPE_TEXT_PLAIN,
         bodyType: typeof(string),
         Summary = StringConstants.RESPONSE_SUMMARY_404,
         Description = StringConstants.RESPONSE_DESCRIPTION_404)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest,
         contentType: StringConstants.CONTENT_TYPE_TEXT_PLAIN,
         bodyType: typeof(string),
         Summary = StringConstants.RESPONSE_SUMMARY_400,
         Description = StringConstants.RESPONSE_DESCRIPTION_400)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError,
         contentType: StringConstants.CONTENT_TYPE_TEXT_PLAIN,
         bodyType: typeof(string),
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
               logger.LogInformation(StringConstants.RESPONSE_REQUEST_PATH_PARAMETER_INVALID);
               return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.BadRequest, StringConstants.RESPONSE_REQUEST_PATH_PARAMETER_INVALID);
            }

            string resourceUri = await _environmentQueries.GetSpecificResourceUri(environmentKey, businessServiceName, dependencyName);

            return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.OK, resourceUri);
         }
         catch (EntityNotFoundException ex)
         {
            logger.LogInformation(ex.Message);
            return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.NotFound, ex.Message);
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.InternalServerError, StringConstants.RESPONSE_GENERIC_ERROR);
         }
      }
   }
}

