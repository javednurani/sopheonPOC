using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Models;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class GetEnvironments
   {
      private readonly static NewtonsoftJsonObjectSerializer _serializer = new NewtonsoftJsonObjectSerializer();
      private readonly IEnvironmentRepository _environmentRepository;
      private IMapper _mapper;

      public GetEnvironments(IEnvironmentRepository environmentRepository, IMapper mapper)
      {
         _environmentRepository = environmentRepository;
         _mapper = mapper;
      }

      [Function(nameof(GetEnvironments))]
      [OpenApiOperation(operationId: nameof(GetEnvironments),
              tags: new[] { "Environments" },
              Summary = "Get all Environments",
              Description = "Get all Environments that are not deleted",
              Visibility = OpenApiVisibilityType.Important)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created,
              contentType: StringConstants.CONTENT_TYPE_APP_JSON,
              bodyType: typeof(IEnumerable<EnvironmentDto>),
              Summary = "200 OK response",
              Description = "OK, 200 response with List of Environments in response body")]
      public async Task<HttpResponseData> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "environments")] HttpRequestData req,
          FunctionContext context)
      {
         var logger = context.GetLogger(nameof(GetEnvironments));
         try
         {
            IEnumerable<Environment> environments = await _environmentRepository.GetEnvironments();

            HttpResponseData response = req.CreateResponse();
            await response.WriteAsJsonAsync(_mapper.Map<IEnumerable<Environment>, IEnumerable<EnvironmentDto>>(environments), _serializer, HttpStatusCode.OK);
            return response;
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            HttpResponseData genericExceptionResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await genericExceptionResponse.WriteStringAsync(StringConstants.RESPONSE_GENERIC_ERROR);
            return genericExceptionResponse;
         }
      }
   }
}
