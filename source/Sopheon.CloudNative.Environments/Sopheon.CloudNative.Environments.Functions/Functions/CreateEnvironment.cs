using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;
using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class CreateEnvironment
   {
      private readonly IEnvironmentRepository _environmentRepository;
      private readonly IMapper _mapper;
      private readonly IValidator<EnvironmentDto> _validator;
      private readonly HttpResponseDataBuilder _responseBuilder;

      public CreateEnvironment(IEnvironmentRepository environmentRepository, IMapper mapper, IValidator<EnvironmentDto> validator, HttpResponseDataBuilder responseBuilder)
      {
         _environmentRepository = environmentRepository;
         _mapper = mapper;
         _validator = validator;
         _responseBuilder = responseBuilder;
      }

      [Function(nameof(CreateEnvironment))]
      [OpenApiOperation(operationId: nameof(CreateEnvironment),
         tags: new[] { "Environments" },
         Summary = "Create an Environment",
         Description = "Create an Environment, with required and optional properties",
         Visibility = OpenApiVisibilityType.Important)]
      [OpenApiRequestBody(contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(EnvironmentDto),
         Required = true,
         Description = "Environment object to be created. Name and Owner required, Description optional")]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(EnvironmentDto),
         Summary = StringConstants.RESPONSE_SUMMARY_201,
         Description = StringConstants.RESPONSE_DESCRIPTION_201)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ErrorDto),
         Summary = StringConstants.RESPONSE_SUMMARY_400,
         Description = StringConstants.RESPONSE_DESCRIPTION_400)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ErrorDto),
         Summary = StringConstants.RESPONSE_SUMMARY_500,
         Description = StringConstants.RESPONSE_DESCRIPTION_500)]

      public async Task<HttpResponseData> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "environments")] HttpRequestData req,
          FunctionContext context)
      {
         ILogger logger = context.GetLogger(nameof(CreateEnvironment));

         string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
         try
         {
            EnvironmentDto data = JsonSerializer.Deserialize<EnvironmentDto>(requestBody, SerializationSettings.JsonSerializerOptions);

            ValidationResult validationResult = await _validator.ValidateAsync(data);
            if(!validationResult.IsValid)
            {
               string validationFailureMessage = validationResult.ToString();
               ErrorDto exception = new ErrorDto
               {
                  StatusCode = (int)HttpStatusCode.BadRequest,
                  Message = validationFailureMessage,
               };
               logger.LogInformation(validationFailureMessage);
               return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.BadRequest, exception);
            }

            Environment environment = new Environment
            {
               Name = data.Name,
               Owner = data.Owner,
               Description = data.Description ?? string.Empty
            };

            // TODO: environments that already exist with name?
            environment = await _environmentRepository.AddEnvironment(environment);
            return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.Created, _mapper.Map<EnvironmentDto>(environment));
         }
         catch (JsonException ex)
         {
            ErrorDto exception = new ErrorDto
            {
               StatusCode = (int)HttpStatusCode.BadRequest,
               Message = StringConstants.RESPONSE_REQUEST_BODY_INVALID,
            };
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.BadRequest, exception);
         }
         catch (Exception ex)
         {
            ErrorDto exception = new ErrorDto
            {
               StatusCode = (int)HttpStatusCode.InternalServerError,
               Message = StringConstants.RESPONSE_GENERIC_ERROR,
            };
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.InternalServerError, exception);
         }
      }
   }
}
