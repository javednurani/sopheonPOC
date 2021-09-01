using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Sopheon.CloudNative.Environments.Domain.Data;
using Sopheon.CloudNative.Environments.Functions.Models;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;
using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class CreateEnvironment
   {
      private readonly EnvironmentContext _environmentContext;
      private IMapper _mapper;

      public CreateEnvironment(EnvironmentContext environmentContext, IMapper mapper)
      {
         _environmentContext = environmentContext;
         _mapper = mapper;
      }

      [Function(nameof(CreateEnvironment))]
      [OpenApiOperation(operationId: "Run", tags: new[] { "Environments~" })]
      [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(EnvironmentDto), Required = true, Description = "Environment object to be created")]
      [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "text/plain", bodyType: typeof(string), Description = "The Created response")]
      public async Task<HttpResponseData> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "CreateEnvironment")] HttpRequestData req,
          FunctionContext context)
      {
         var logger = context.GetLogger(nameof(CreateEnvironment));

         logger.LogInformation("C# HTTP trigger function processed a CreateEnvironment request.");

         string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
         try
         {
            EnvironmentDto data = JsonConvert.DeserializeObject<EnvironmentDto>(requestBody);
            if (string.IsNullOrEmpty(data.Name))
            {
               logger.LogInformation("Request missing required Name field");
               // TODO: Descriptive response message
               return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            if (data.Owner == Guid.Empty)
            {
               logger.LogInformation("Request missing required Owner field");
               // TODO: Descriptive response message
               return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            Environment environment = new Environment()
            {
               EnvironmentKey = Guid.NewGuid(),
               Name = data.Name,
               Owner = data.Owner,
               Description = data.Description
            };

            // TODO: environments that already exist with name?
            _environmentContext.Environments.Add(environment);
            await _environmentContext.SaveChangesAsync();

            HttpResponseData createdResponse = req.CreateResponse(HttpStatusCode.Created);
            await createdResponse.WriteAsJsonAsync(_mapper.Map<Environment, EnvironmentDto>(environment));

            return createdResponse;
         }
         catch (JsonSerializationException ex)
         {
            logger.LogInformation("Exception deserializing request data");
            return req.CreateResponse(HttpStatusCode.BadRequest);
         }
         catch (Exception ex)
         {
            return req.CreateResponse(HttpStatusCode.BadRequest);
         }
      }
   }
}
