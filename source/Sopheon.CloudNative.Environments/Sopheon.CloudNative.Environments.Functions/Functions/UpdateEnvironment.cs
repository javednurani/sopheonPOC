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
using Sopheon.CloudNative.Environments.Domain.Exceptions;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class UpdateEnvironment
   {
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
      [OpenApiParameter(name: "key",
         Type = typeof(Guid),
         Required = true,
         Description = "The key of the Environment to update.",
         Summary = "The key of the Environment to update.")]
      [OpenApiRequestBody(contentType: "application/json",
         bodyType: typeof(EnvironmentDto),
         Required = true,
         Description = "Environment object to be updated.")]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(EnvironmentDto),
         Summary = StringConstants.RESPONSE_SUMMARY_200,
         Description = StringConstants.RESPONSE_DESCRIPTION_200)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ErrorDto),
         Summary = StringConstants.RESPONSE_SUMMARY_400,
         Description = StringConstants.RESPONSE_DESCRIPTION_400)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ErrorDto),
         Summary = StringConstants.RESPONSE_SUMMARY_404,
         Description = StringConstants.RESPONSE_DESCRIPTION_404)]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ErrorDto),
         Summary = StringConstants.RESPONSE_SUMMARY_500,
         Description = StringConstants.RESPONSE_DESCRIPTION_500)]

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
               ErrorDto exception = new ErrorDto
               {
                  StatusCode = (int)HttpStatusCode.BadRequest,
                  Message = StringConstants.RESPONSE_REQUEST_ENVIRONMENTKEY_INVALID,
               };
               logger.LogInformation(StringConstants.RESPONSE_REQUEST_ENVIRONMENTKEY_INVALID);
               return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.BadRequest, exception);
            }
            
            EnvironmentDto data = JsonSerializer.Deserialize<EnvironmentDto>(requestBody, SerializationSettings.JsonSerializerOptions);

            ValidationResult validationResult = await _validator.ValidateAsync(data);
            if (!validationResult.IsValid)
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
               EnvironmentKey = environmentKey,
               Name = data.Name,
               Owner = data.Owner,
               Description = data.Description ?? string.Empty,
            };

            environment = await _environmentRepository.UpdateEnvironment(environment);
            return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.OK, _mapper.Map<EnvironmentDto>(environment));
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
         catch (EntityNotFoundException ex)
         {
            ErrorDto exception = new ErrorDto
            {
               StatusCode = (int)HttpStatusCode.NotFound,
               Message = ex.Message,
            };
            logger.LogInformation(ex.Message);
            return await _responseBuilder.BuildWithJsonBody(req, HttpStatusCode.NotFound, exception);
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
