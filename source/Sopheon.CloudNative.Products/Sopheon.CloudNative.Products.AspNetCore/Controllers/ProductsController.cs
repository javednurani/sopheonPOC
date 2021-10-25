using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Products.AspNetCore.Models;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.AspNetCore.Controllers
{
   public class ProductsController : EnvironmentScopedControllerBase
   {
      private readonly ILogger<ProductsController> _logger;
      private readonly ProductManagementContext _dbContext;
      private readonly IMapper _mapper;

      public ProductsController(ILogger<ProductsController> logger,
         ProductManagementContext dbContext,
         IMapper mapper)
      {
         _logger = logger;
         _dbContext = dbContext;
         _mapper = mapper;
      }

      [ProducesResponseType(StatusCodes.Status200OK)]
      [HttpGet]
      public async Task<IEnumerable<ProductDto>> Get()
      {
         //ClaimsPrincipal user = HttpContext.User;
         var query = _dbContext.Products
               .AsNoTracking()
               .ProjectTo<ProductDto>(_mapper.ConfigurationProvider);

         //var debug = query.ToQueryString();

         return await query.ToArrayAsync();
      }

      [HttpGet("{key}/Items")]
      public async Task<IActionResult> GetItems(string key) 
      {
         var query = _dbContext.Products
             .AsNoTracking()
             .Where(p => p.Key == key)
             .Select(p => p.Items)
             .ProjectTo<ProductItemDto>(_mapper.ConfigurationProvider);

         var results = await query.ToArrayAsync();
         return Ok(results);
      }

      [HttpPatch("{key}")]
      public async Task<IActionResult> Patch(string key, [FromBody] JsonPatchDocument<ProductPatchDto> patchDocument)
      {
         if (patchDocument.Operations.Count == 0) 
         {
            return NoContent();
         }

         //ClaimsPrincipal user = HttpContext.User;
         var test = await _dbContext.Products.ToListAsync();
         Product productFromDatabase = await _dbContext.Products.SingleOrDefaultAsync(p => p.Key == key);
         if (productFromDatabase == null) 
         {
            return NotFound();
         }

         ProductPatchDto productDto = _mapper.Map<ProductPatchDto>(productFromDatabase);

         patchDocument.ApplyTo(productDto); //Apply the patch to that DTO. 
         _mapper.Map(productDto, productFromDatabase); //Use automapper to map the DTO back ontop of the database object. 

         await _dbContext.SaveChangesAsync();

         return NoContent();
      }

      [HttpPost]
      [Route("SeedRandom")]
      public async Task<IEnumerable<Product>> PostSeedProducts()
      {
         ClaimsPrincipal user = HttpContext.User;


         var newProduct = new Product()
         {
            Name = Guid.NewGuid().ToString(),
            Key = Guid.NewGuid().ToString(),
            Items = new List<ProductItem>() 
            {
               new ProductItem()
               {
                  Name = Guid.NewGuid().ToString(),
                  ProductItemTypeId = (int)SystemManagedProductItemTypeIds.Task
               }
            },
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

         return await _dbContext.Products
               //.Where(filterExpression)
               .AsNoTracking()
               .ToArrayAsync();
      }
   }
}
