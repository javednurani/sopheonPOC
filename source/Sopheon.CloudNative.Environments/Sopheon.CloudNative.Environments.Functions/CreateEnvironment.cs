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
using Sopheon.CloudNative.Environments.Functions.Models;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;
using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class CreateEnvironment
   {
      // Cloud-1484, we are defining ObjectSerializer to be used, per Function class
      // this is due to unit test context not having a serializer configured, if we use the below line to configure serializer for production context
      // Ideally, we would use this line in Program.cs :: main() : .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
      private readonly static NewtonsoftJsonObjectSerializer _serializer = new NewtonsoftJsonObjectSerializer();
      private readonly IEnvironmentRepository _environmentRepository;
      private readonly IMapper _mapper;
      private readonly IValidator<EnvironmentDto> _validator;

      public CreateEnvironment(IEnvironmentRepository environmentRepository, IMapper mapper, IValidator<EnvironmentDto> validator)
      {
         _environmentRepository = environmentRepository;
         _mapper = mapper;
         _validator = validator;
      }

      [Function(nameof(CreateEnvironment))]
      [OpenApiOperation(operationId: nameof(CreateEnvironment),
         tags: new[] { "Environments" },
         Summary = "Create an Environment",
         Description = "Create an Environment, with required and optional properties",
         Visibility = OpenApiVisibilityType.Important)]
      [OpenApiRequestBody(contentType: "application/json",
         bodyType: typeof(EnvironmentDto),
         Required = true,
         Description = "Environment object to be created. Name and Owner required, Description optional")]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created,
         contentType: "application/json",
         bodyType: typeof(EnvironmentDto),
         Summary = "201 Created response",
         Description = "Created, 201 response with Environment in response body")]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest,
         contentType: "application/json",
         bodyType: typeof(string),
         Summary = "400 Bad Request response",
         Description = "Bad Request, 400 response with error message in response body")]

      public async Task<HttpResponseData> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "environments")] HttpRequestData req,
          FunctionContext context)
      {
         var logger = context.GetLogger(nameof(CreateEnvironment));

         string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
         try
         {
            EnvironmentDto data = JsonConvert.DeserializeObject<EnvironmentDto>(requestBody);

            ValidationResult validationResult = await _validator.ValidateAsync(data);
            if(!validationResult.IsValid)
            {
               string validationFailureMessage = validationResult.ToString();
               logger.LogInformation(validationFailureMessage);
               HttpResponseData response = req.CreateResponse(HttpStatusCode.BadRequest);
               await response.WriteStringAsync(validationFailureMessage);
               return response;
            }

            Environment environment = new Environment
            {
               Name = data.Name,
               Owner = data.Owner,
               Description = data.Description ?? string.Empty
            };

            // TODO: environments that already exist with name?
            environment = await _environmentRepository.AddEnvironment(environment);

            HttpResponseData createdResponse = req.CreateResponse();
            await createdResponse.WriteAsJsonAsync(_mapper.Map<Environment, EnvironmentDto>(environment), _serializer, HttpStatusCode.Created);
            return createdResponse;
         }
         catch (JsonReaderException ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            HttpResponseData deserializeExceptionResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await deserializeExceptionResponse.WriteStringAsync("Request body was invalid.");
            return deserializeExceptionResponse;
         }
         catch (JsonSerializationException ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            HttpResponseData deserializeExceptionResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await deserializeExceptionResponse.WriteStringAsync($"Request body was invalid. Is {nameof(EnvironmentDto.Owner)} field a valid GUID?");
            return deserializeExceptionResponse;
         }
         catch (Exception ex)
         {
            // TODO: should this be a 400 or 500?  The try block encompasses database and network I/O that can throw exceptions
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            HttpResponseData genericExceptionResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await genericExceptionResponse.WriteStringAsync("Something went wrong. Please try again later.");
            return genericExceptionResponse;
         }
      }
   }
}
