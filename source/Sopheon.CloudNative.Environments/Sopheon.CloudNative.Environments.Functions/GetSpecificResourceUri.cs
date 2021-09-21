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
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
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
            Route = "GetEnvironmentResourceBindingUri({environmentKey}, {businessServiceKey}, {dependencyKey})")] HttpRequestData req,
          FunctionContext context, string environmentKey, string businessServiceKey, string dependencyKey)
      {
         var logger = context.GetLogger(nameof(GetSpecificResourceUri));
         try
         {
            // TODO, other validation eg minLength?
            if (string.IsNullOrEmpty(environmentKey) || string.IsNullOrEmpty(businessServiceKey) || string.IsNullOrEmpty(dependencyKey))
            {
               logger.LogInformation(StringConstants.RESPONSE_REQUEST_PATH_PARAMETER_MISSING);
               return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.BadRequest, StringConstants.RESPONSE_REQUEST_PATH_PARAMETER_MISSING);
            }

            string resourceUri = await _environmentQueries.GetSpecificResourceUri(environmentKey, businessServiceKey, dependencyKey);

            return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.OK, resourceUri);
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

