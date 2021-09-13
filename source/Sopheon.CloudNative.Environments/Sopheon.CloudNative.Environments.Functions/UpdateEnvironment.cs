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
using Sopheon.CloudNative.Environments.Domain.Exceptions;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class UpdateEnvironment
   {
      // Cloud-1484, we are defining ObjectSerializer to be used, per Function class
      // this is due to unit test context not having a serializer configured, if we use the below line to configure serializer for production context
      // Ideally, we would use this line in Program.cs :: main() : .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
      private readonly static NewtonsoftJsonObjectSerializer _serializer = new NewtonsoftJsonObjectSerializer();
      private readonly IEnvironmentRepository _environmentRepository;
      private readonly IMapper _mapper;
      private readonly IValidator<EnvironmentDto> _validator;
      private readonly HttpResponseDataBuilder _responseBuilder;

      public UpdateEnvironment(IEnvironmentRepository environmentRepository, IMapper mapper, IValidator<EnvironmentDto> validator, HttpResponseDataBuilder responseBuilder)
      {
         _environmentRepository = environmentRepository;
         _mapper = mapper;
         _validator = validator;
         _responseBuilder = responseBuilder;
      }

      [Function(nameof(UpdateEnvironment))]
      [OpenApiOperation(operationId: nameof(UpdateEnvironment),
         tags: new[] { "Environments" },
         Summary = "Update an Environment",
         Description = "Update an Environment's properties. Anything except IsDeleted, EnvironmentKey, and EnvironmentId can be changed.",
         Visibility = OpenApiVisibilityType.Important)]
      [OpenApiRequestBody(contentType: "application/json",
         bodyType: typeof(EnvironmentDto),
         Required = true,
         Description = "Environment object to be updated.")]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK,
         contentType: "application/json",
         bodyType: typeof(EnvironmentDto),
         Summary = "200 OK response",
         Description = "OK, 200 response with Environment in response body")]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest,
         contentType: "application/json",
         bodyType: typeof(string),
         Summary = "400 Bad Request response",
         Description = "Bad Request, 400 response with error message in response body")]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound,
         contentType: "application/json",
         bodyType: typeof(string),
         Summary = "404 Not Found response",
         Description = "Not Found, 404 response with error message in response body.")]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError,
         contentType: "application/json",
         bodyType: typeof(string),
         Summary = "500 Internal Server Error",
         Description = "Internal Server Error, 500 response with error message in response body")]

      public async Task<HttpResponseData> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "environments/{key}")] HttpRequestData req,
          FunctionContext context, string key)
      {
         var logger = context.GetLogger(nameof(UpdateEnvironment));

         string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

         try
         {
            Guid environmentKey;
            bool validKey = Guid.TryParse(key, out environmentKey);
            if (!validKey || environmentKey == Guid.Empty)
            {
               logger.LogInformation(StringConstants.RESPONSE_REQUEST_ENVIRONMENTKEY_INVALID);
               return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.BadRequest, StringConstants.RESPONSE_REQUEST_ENVIRONMENTKEY_INVALID);
            }
            EnvironmentDto data = JsonConvert.DeserializeObject<EnvironmentDto>(requestBody);

            ValidationResult validationResult = await _validator.ValidateAsync(data);
            if (!validationResult.IsValid)
            {
               string validationFailureMessage = validationResult.ToString();
               logger.LogInformation(validationFailureMessage);
               return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.BadRequest, validationFailureMessage);
            }

            Environment environment = new Environment
            {
               EnvironmentKey = environmentKey,
               Name = data.Name,
               Owner = data.Owner,
               Description = data.Description ?? string.Empty,
            };

            environment = await _environmentRepository.UpdateEnvironment(environment);
            return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.OK, _mapper.Map<Environment, EnvironmentDto>(environment), _serializer);
         }
         catch (JsonException ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithStringBody(req, HttpStatusCode.BadRequest, StringConstants.RESPONSE_REQUEST_BODY_INVALID);
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
