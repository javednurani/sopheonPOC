using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.AspNetCore.Controllers
{
   public class AttributesController : EnvironmentScopedControllerBase
   {
      private readonly ILogger<AttributesController> _logger;
      private readonly ProductManagementContext _dbContext;

      public AttributesController(ILogger<AttributesController> logger, ProductManagementContext dbContext)
      {
         _logger = logger;
         _dbContext = dbContext;
      }

      [HttpGet]
      public async Task<IEnumerable<Domain.Attribute>> Get()
      {
         ClaimsPrincipal user = HttpContext.User;

         return await _dbContext.Attributes
               .AsNoTracking()
               .ToArrayAsync();
      }

      [HttpPost]
      [Route("SeedRandom")]
      public async Task<IEnumerable<Product>> PostSeedRandom()
      {
         ClaimsPrincipal user = HttpContext.User;

         List<AttributeValueType> attributeTypes = await _dbContext.AttributeValueType.ToListAsync();
         AttributeValueType randomAttributeType = attributeTypes[new Random().Next(0, attributeTypes.Count)];

         var newAttribute = new Domain.Attribute()
         {
            Name = $"{randomAttributeType.Name}_{Guid.NewGuid()}",
            AttributeValueTypeId = randomAttributeType.AttributeValueTypeId
         };
         _dbContext.Attributes.Add(newAttribute);

         await _dbContext.SaveChangesAsync();

         await _dbContext.SaveChangesAsync();

         return await _dbContext.Products
               .AsNoTracking()
               .ToArrayAsync();
      }
   }
}
