using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Environments.Data;
using Sopheon.CloudNative.Environments.Domain.Models;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class GetBusinessServiceDependency
   {
      private readonly EnvironmentContext _context;
      private readonly IMapper _mapper;
      private readonly HttpResponseDataBuilder _responseBuilder;

      public GetBusinessServiceDependency(EnvironmentContext context, IMapper mapper, HttpResponseDataBuilder responseBuilder)
      {
         _context = context;
         _mapper = mapper;
         _responseBuilder = responseBuilder;
      }

      [Function(nameof(GetBusinessServiceDependency))]
      [OpenApiOperation(operationId: nameof(GetBusinessServiceDependency),
         tags: new[] { "Resources" },
         Summary = "Get all Resource Types",
         Description = "Get all Resource types",
         Visibility = OpenApiVisibilityType.Important)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(IEnumerable<ResourceTypeDto>),
         Summary = StringConstants.RESPONSE_SUMMARY_200,
         Description = StringConstants.RESPONSE_DESCRIPTION_200)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ErrorDto),
         Summary = StringConstants.RESPONSE_SUMMARY_500,
         Description = StringConstants.RESPONSE_DESCRIPTION_500)]
      public async Task<HttpResponseData> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ResourceTypes")] HttpRequestData req,
          FunctionContext context)
      {
         ILogger logger = context.GetLogger(nameof(GetBusinessServiceDependency));
         try
         {
            IEnumerable<DomainResourceType> resources = await _context.DomainResourceTypes.ToListAsync();

            return await _responseBuilder.BuildWithJsonBodyAsync(req, HttpStatusCode.OK, _mapper.Map<IEnumerable<ResourceTypeDto>>(resources));
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithErrorBodyAsync(req, HttpStatusCode.InternalServerError, StringConstants.RESPONSE_GENERIC_ERROR);
         }
      }
   }
}
