using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Environments.Domain.Models;
using Sopheon.CloudNative.Environments.Domain.Queries;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Validators;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class GetResourceUrisByBusinessServiceDependency
   {
      private readonly IEnvironmentQueries _environmentQueries;
      private readonly IRequiredStringValidator _validator;
      private IMapper _mapper;
      private readonly HttpResponseDataBuilder _responseBuilder;

      public GetResourceUrisByBusinessServiceDependency(IEnvironmentQueries environmentQueries, IRequiredStringValidator validator, IMapper mapper, HttpResponseDataBuilder responseBuilder)
      {
         _environmentQueries = environmentQueries;
         _validator = validator;
         _mapper = mapper;
         _responseBuilder = responseBuilder;
      }

      [Function(nameof(GetResourceUrisByBusinessServiceDependency))]
      [OpenApiOperation(operationId: nameof(GetResourceUrisByBusinessServiceDependency),
         tags: new[] { nameof(Environment), nameof(Resource), nameof(BusinessService), nameof(BusinessServiceDependency), nameof(EnvironmentResourceBinding) },
         Summary = "Get Resource URIs for a BusinessServiceDependency, across Environments",
         Description = "Get Resource URIs for a BusinessServiceDependency, across Environments",
         Visibility = OpenApiVisibilityType.Important)]
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
         bodyType: typeof(IEnumerable<string>),
         Summary = StringConstants.RESPONSE_SUMMARY_200,
         Description = StringConstants.RESPONSE_DESCRIPTION_200)]
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
            Route = "BusinessService/{businessServiceName}/GetEnvironmentResourceBindingUris({dependencyName})")] HttpRequestData req,
          FunctionContext context, string businessServiceName, string dependencyName)
      {
         var logger = context.GetLogger(nameof(GetResourceUrisByBusinessServiceDependency));
         try
         {
            ValidationResult validationResultBusinessServiceName = await _validator.ValidateAsync(businessServiceName);
            ValidationResult validationResultDependencyname = await _validator.ValidateAsync(dependencyName);

            if (!validationResultBusinessServiceName.IsValid || !validationResultDependencyname.IsValid)
            {
               logger.LogInformation(StringConstants.RESPONSE_REQUEST_PATH_PARAMETER_MISSING);
               return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.BadRequest, StringConstants.RESPONSE_REQUEST_PATH_PARAMETER_MISSING);
            }

            IEnumerable<string> resourceUris = await _environmentQueries.GetResourceUrisByBusinessServiceDependency(businessServiceName, dependencyName);

            return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.OK, resourceUris);
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.InternalServerError, StringConstants.RESPONSE_GENERIC_ERROR);
         }
      }
   }
}
