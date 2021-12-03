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
using Sopheon.CloudNative.Environments.Data;
using Sopheon.CloudNative.Environments.Domain.Models;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;
using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class RegisterResource
   {
      private readonly EnvironmentContext _context;
      private readonly IMapper _mapper;
      private readonly IValidator<ResourceRegistrationDto> _validator;
      private readonly HttpResponseDataBuilder _responseBuilder;

      public RegisterResource(EnvironmentContext context, IMapper mapper, IValidator<ResourceRegistrationDto> validator, HttpResponseDataBuilder responseBuilder)
      {
         _context = context;
         _mapper = mapper;
         _validator = validator;
         _responseBuilder = responseBuilder;
      }

      [Function(nameof(RegisterResource))]
      [OpenApiOperation(operationId: nameof(RegisterResource),
         tags: new[] { "Resources" },
         Summary = "Register a Resource",
         Description = "Register a Resource, with required and optional properties",
         Visibility = OpenApiVisibilityType.Important)]
      [OpenApiRequestBody(contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ResourceRegistrationDto),
         Required = true,
         Description = "Resource details to be registered")]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created,
         contentType: StringConstants.CONTENT_TYPE_APP_JSON,
         bodyType: typeof(ResourceDto),
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
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Resource")] HttpRequestData req,
          FunctionContext context)
      {
         ILogger logger = context.GetLogger(nameof(RegisterResource));

         string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
         try
         {
            ResourceRegistrationDto data = JsonSerializer.Deserialize<ResourceRegistrationDto>(requestBody, SerializationSettings.JsonSerializerOptions);

            ValidationResult validationResult = await _validator.ValidateAsync(data);
            if(!validationResult.IsValid)
            {
               string validationMessage = validationResult.ToString();
               logger.LogInformation(validationMessage);
               return await _responseBuilder.BuildWithErrorBodyAsync(req, HttpStatusCode.BadRequest, validationMessage);
            }

            Resource resource = new Resource
            {
               Uri = data.Uri,
               DomainResourceTypeId = data.ResourceTypeId
            };

            _context.Resources.Add(resource);

            await _context.SaveChangesAsync();

            return await _responseBuilder.BuildWithJsonBodyAsync(req, HttpStatusCode.Created, _mapper.Map<ResourceDto>(resource));
         }
         catch (JsonException ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithErrorBodyAsync(req, HttpStatusCode.BadRequest, StringConstants.RESPONSE_REQUEST_BODY_INVALID);
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            return await _responseBuilder.BuildWithErrorBodyAsync(req, HttpStatusCode.InternalServerError, StringConstants.RESPONSE_GENERIC_ERROR);
         }
      }
   }
}
