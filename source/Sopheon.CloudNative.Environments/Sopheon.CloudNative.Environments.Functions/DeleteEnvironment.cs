using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core.Serialization;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;
using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class DeleteEnvironment
   {
      // Cloud-1484, we are defining ObjectSerializer to be used, per Function class
      // this is due to unit test context not having a serializer configured, if we use the below line to configure serializer for production context
      // Ideally, we would use this line in Program.cs :: main() : .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
      private readonly static NewtonsoftJsonObjectSerializer _serializer = new NewtonsoftJsonObjectSerializer();
      private readonly IEnvironmentRepository _environmentRepository;
      private readonly IMapper _mapper;
      private readonly IValidator<EnvironmentDto> _validator;
      private readonly HttpResponseDataBuilder _responseBuilder;
    
      public DeleteEnvironment(IEnvironmentRepository environmentRepository, IMapper mapper, IValidator<EnvironmentDto> validator, HttpResponseDataBuilder responseBuilder)
      {
         _environmentRepository = environmentRepository;
         _mapper = mapper;
         _validator = validator;
         _responseBuilder = responseBuilder;
      }

      [Function(nameof(DeleteEnvironment))]
      [OpenApiOperation(operationId: nameof(DeleteEnvironment),
         tags: new[] { "Environments" },
         Summary = "Delete an Environment",
         Description = "Delete an Environment",
         Visibility = OpenApiVisibilityType.Important)]
      [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent,
         Summary = "204 No Content response",
         Description = "No Content, successful Delete")]
      [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound,
         Summary = "404 Not Found response",
         Description = "Not Found, Environment to be deleted was not found")]
      [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest,
         Summary = "400 Bad Request response",
         Description = "Bad Request")]

      public async Task<HttpResponseData> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "environments/{key}")] HttpRequestData req,
          FunctionContext context, string key)
      {
         var logger = context.GetLogger(nameof(DeleteEnvironment));

         try
         {
            Guid environmentKey;
            bool validKey = Guid.TryParse(key, out environmentKey);
            if (!validKey || environmentKey == Guid.Empty)
            {
               string keyNotGuidMessage = "The EnvironmentKey must be a valid Guid";
               logger.LogInformation(keyNotGuidMessage);
               return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.BadRequest, keyNotGuidMessage);
            }

            Environment environment = new Environment
            {
               EnvironmentKey = environmentKey
            };

            bool deleteSuccess = await _environmentRepository.DeleteEnvironment(environment);

            HttpStatusCode statusCode = deleteSuccess ? HttpStatusCode.NoContent : HttpStatusCode.NotFound;

            return _responseBuilder.BuildWithoutBody(req, statusCode);

         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.InternalServerError, "Something went wrong. Please try again later.");
         }
      }
   }
}
