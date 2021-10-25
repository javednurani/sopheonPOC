using System;
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
using System.Linq;
using System.Linq.Expressions;

namespace Sopheon.CloudNative.Products.Functions
{
   public class ProductsFunctions
   {
      private readonly ProductManagementContext _dbContext;

      public ProductsFunctions(ProductManagementContext dbContext)
      {
         _dbContext = dbContext;
      }

      [Function(nameof(GetProducts))]
      public async Task<HttpResponseData> GetProducts(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Environments/{EnvironmentId}/Products")] HttpRequestData req,
         string environmentId,
         string productId,
         FunctionContext executionContext)
      {
         var logger = executionContext.GetLogger(nameof(GetProducts));
         logger.LogInformation("C# HTTP trigger function processed a request.");

         Expression<Func<Product, bool>> filterExpression = p => p.IntAttributeValues.Any(s => s.AttributeId == -2 && s.Value > 20);

         List<Product> products =
            await _dbContext.Products
               .Where(filterExpression)
               .AsNoTracking()
               .ToListAsync();
         var response = req.CreateResponse(HttpStatusCode.OK);
         await response.WriteAsJsonAsync(products);

         return response;
      }

      [Function(nameof(GetProduct))]
      public async Task<HttpResponseData> GetProduct(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Environments/{EnvironmentId}/Products/{ProductId}")] HttpRequestData req,
         string environmentId,
         int productId,
         FunctionContext executionContext)
      {
         var logger = executionContext.GetLogger(nameof(GetProducts));
         logger.LogInformation("C# HTTP trigger function processed a request.");

         Product product =
            await _dbContext.Products
               // Owned entities included by default; lots of left outer join queryies
               //.Include(p => p.IntAttributeValues)
               //.Include(p => p.DecimalAttributeValues)
               //.Include(p => p.MoneyAttributeValues)
               //   .ThenInclude(money => money.Value)
               //.Include(p => p.StringAttributeValues)
               //.Include(p => p.UtcDateTimeAttributeValues)
               .AsNoTracking()
               .SingleAsync(p => p.Id == productId);
         // TODO: Money Value retrieved but not serialized; need to investigate why
         var response = req.CreateResponse(HttpStatusCode.OK);
         await response.WriteAsJsonAsync(product);

         return response;
      }

      [Function(nameof(PostProducts))]
      public async Task<HttpResponseData> PostProducts(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Environments/{EnvironmentId}/Products")] HttpRequestData req,
         FunctionContext executionContext)
      {
         var logger = executionContext.GetLogger(nameof(AttributeFunctions));
         logger.LogInformation("C# HTTP trigger function processed a request.");

         string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

         ProductCreateDto attributeDto = JsonSerializer.Deserialize<ProductCreateDto>(requestBody); // TODO: Model Deserialization and Validation Error Handling
         var newProduct = new Product()
         {
            Name = attributeDto.Name
         };
         _dbContext.Products.Add(newProduct);

         await _dbContext.SaveChangesAsync();

         var createdProduct = await _dbContext.Products.SingleAsync(s => s.Id == newProduct.Id);

         var response = req.CreateResponse(HttpStatusCode.Created);
         await response.WriteAsJsonAsync(createdProduct);

         return response;
      }

      [Function(nameof(PostSeedProducts))]
      public async Task<HttpResponseData> PostSeedProducts(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Environments/{EnvironmentId}/SeedRandomProduct")] HttpRequestData req,
         FunctionContext executionContext)
      {
         var logger = executionContext.GetLogger(nameof(AttributeFunctions));
         logger.LogInformation("C# HTTP trigger function processed a request.");


         var newProduct = new Product()
         {
            Name = Guid.NewGuid().ToString(),
            IntAttributeValues = new List<Int32AttributeValue>()
            {
               new Int32AttributeValue()
               {
                  AttributeId = -2,
                  Value = new Random().Next(0, 100)
               }
            },
            MoneyAttributeValues = new List<MoneyAttributeValue>()
            {
               new MoneyAttributeValue()
               {
                  AttributeId = -1,
                  Value = new MoneyValue()
                  {
                     CurrencyCode = "USD",
                     Value = 999
                  }
               }
            },
            UtcDateTimeAttributeValues = new List<UtcDateTimeAttributeValue>()
            {
               new UtcDateTimeAttributeValue()
               {
                  AttributeId = -4,
                  Value = DateTime.UtcNow
               }
            },
            StringAttributeValues = new List<StringAttributeValue>()
            {
               new StringAttributeValue()
               {
                  AttributeId = -3,
                  Value = "Food and Bev"
               }
            }
         };
         _dbContext.Products.Add(newProduct);

         await _dbContext.SaveChangesAsync();

         List<Product> products = await _dbContext.Products.ToListAsync();
         var response = req.CreateResponse(HttpStatusCode.Created);
         await response.WriteAsJsonAsync(products);


         return response;
      }

      [Function(nameof(PostProductItems))]
      public async Task<HttpResponseData> PostProductItems(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Environments/{EnvironmentId}/Products/{ProductId}/Items")] HttpRequestData req,
         int productId,
         FunctionContext executionContext)
      {
         var logger = executionContext.GetLogger(nameof(ProductsFunctions));
         logger.LogInformation("C# HTTP trigger function processed a request.");

         string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

         ProductItemCreateDto productItemCreateDto = JsonSerializer.Deserialize<ProductItemCreateDto>(requestBody); // TODO: Model Deserialization and Validation Error Handling

         ProductItem newProductItem = new ProductItem()
         {
            Name = productItemCreateDto.Name,
            ProductItemTypeId = productItemCreateDto.ProductItemTypeId
         };

         Product product = await _dbContext.Products
            .Include(p => p.Items)
            .SingleAsync(p => p.Id == productId);

         product.Items.Add(newProductItem);

         await _dbContext.SaveChangesAsync();

         var response = req.CreateResponse(HttpStatusCode.Created);
         await response.WriteAsJsonAsync(newProductItem);

         return response;
      }
   }
}
