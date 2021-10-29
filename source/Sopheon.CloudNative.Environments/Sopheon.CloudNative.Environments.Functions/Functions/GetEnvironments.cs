using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class GetEnvironments
   {
      private readonly IEnvironmentRepository _environmentRepository;
      private IMapper _mapper;
      private readonly HttpResponseDataBuilder _responseBuilder;

      public GetEnvironments(IEnvironmentRepository environmentRepository, IMapper mapper, HttpResponseDataBuilder responseBuilder)
      {
         _environmentRepository = environmentRepository;
         _mapper = mapper;
         _responseBuilder = responseBuilder;
      }

      [Function(nameof(GetEnvironments))]
      [OpenApiOperation(operationId: nameof(GetEnvironments),
         tags: new[] { "Environments" },
         Summary = "Get all Environments",
         Description = "Get all Environments that are not deleted",
         Visibility = OpenApiVisibilityType.Important)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(IEnumerable<EnvironmentDto>),
         Summary = StringConstants.RESPONSE_SUMMARY_200,
         Description = StringConstants.RESPONSE_DESCRIPTION_200)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ErrorDto),
         Summary = StringConstants.RESPONSE_SUMMARY_500,
         Description = StringConstants.RESPONSE_DESCRIPTION_500)]
      public async Task<HttpResponseData> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "environments")] HttpRequestData req,
          FunctionContext context)
      {
         ILogger logger = context.GetLogger(nameof(GetEnvironments));
         try
         {
            IEnumerable<Environment> environments = await _environmentRepository.GetEnvironments();

            return await _responseBuilder.BuildWithJsonBodyAsync(req, HttpStatusCode.OK, _mapper.Map<IEnumerable<EnvironmentDto>>(environments));
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithErrorBodyAsync(req, new ErrorDto((int)(int)HttpStatusCode.InternalServerError, StringConstants.RESPONSE_GENERIC_ERROR));
         }
      }
   }
}
