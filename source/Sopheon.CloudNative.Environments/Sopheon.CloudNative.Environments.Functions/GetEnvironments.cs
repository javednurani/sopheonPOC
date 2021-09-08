using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Collections.Generic;
using Sopheon.CloudNative.Environments.Domain.Models;
using AutoMapper;
using Sopheon.CloudNative.Environments.Functions.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using System.Net;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Azure.Core.Serialization;
using System;
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
              contentType: "application/json",
              bodyType: typeof(List<EnvironmentDto>),
              Summary = "200 OK response",
              Description = "OK, 200 response with List of Environments in response body")]      
      public async Task<HttpResponseData> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "environments")] HttpRequestData req,
          FunctionContext context)
      {
         var logger = context.GetLogger(nameof(GetEnvironments));
         try
         {
            List<Environment> environments = await _environmentRepository.GetEnvironments();

            HttpResponseData response = req.CreateResponse();
            await response.WriteAsJsonAsync(_mapper.Map<List<Environment>, List<EnvironmentDto>>(environments), _serializer, HttpStatusCode.OK);
            return response;
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            HttpResponseData genericExceptionResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await genericExceptionResponse.WriteStringAsync("Something went wrong. Please try again later.");
            return genericExceptionResponse;
         }
      }
   }
}
