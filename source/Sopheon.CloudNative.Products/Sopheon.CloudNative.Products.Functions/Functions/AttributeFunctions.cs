using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Products.Api.Models;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.Functions
{
   public class AttributeFunctions
   {
      private readonly ProductManagementContext _dbContext;

      public AttributeFunctions(ProductManagementContext dbContext)
      {
         _dbContext = dbContext;
      }

      [Function(nameof(GetAttributeValueTypes))]
      public async Task<HttpResponseData> GetAttributeValueTypes(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Environments/{EnvironmentId}/AttributeValueTypes")] HttpRequestData req,
         FunctionContext executionContext)
      {
         var logger = executionContext.GetLogger(nameof(AttributeFunctions));
         logger.LogInformation("C# HTTP trigger function processed a request.");

         List<AttributeValueType> attributes = await _dbContext.AttributeValueType.ToListAsync();
         var response = req.CreateResponse(HttpStatusCode.OK);
         await response.WriteAsJsonAsync(attributes);

         return response;
      }

      [Function(nameof(GetAttributes))]
      public async Task<HttpResponseData> GetAttributes(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Environments/{EnvironmentId}/Attributes")] HttpRequestData req,
          FunctionContext executionContext)
      {
         var logger = executionContext.GetLogger(nameof(AttributeFunctions));
         logger.LogInformation("C# HTTP trigger function processed a request.");

         List<Attribute> attributes = await _dbContext.Attributes.ToListAsync();
         var response = req.CreateResponse(HttpStatusCode.OK);
         await response.WriteAsJsonAsync(attributes);


         return response;
      }

      [Function(nameof(PostAttributes))]
      public async Task<HttpResponseData> PostAttributes(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Environments/{EnvironmentId}/Attributes")] HttpRequestData req,
         FunctionContext executionContext)
      {
         var logger = executionContext.GetLogger(nameof(AttributeFunctions));
         logger.LogInformation("C# HTTP trigger function processed a request.");

         string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

         AttributeCreateDto attributeDto = JsonSerializer.Deserialize<AttributeCreateDto>(requestBody); // TODO: Model Deserialization and Validation Error Handling
         var newAttribute = new Attribute()
         {
            Name = attributeDto.Name,
            AttributeValueTypeId = attributeDto.AttributeValueTypeId
         };
         _dbContext.Attributes.Add(newAttribute);

         await _dbContext.SaveChangesAsync();

         List<Domain.Attribute> attributes = await _dbContext.Attributes.ToListAsync();
         var response = req.CreateResponse(HttpStatusCode.Created);
         await response.WriteAsJsonAsync(attributes);


         return response;
      }
   }
}
