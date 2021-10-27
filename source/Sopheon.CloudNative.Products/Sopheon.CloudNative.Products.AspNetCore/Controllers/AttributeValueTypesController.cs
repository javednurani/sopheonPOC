using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.AspNetCore.Controllers
{
   public class AttributeValueTypesController : EnvironmentScopedControllerBase
   {
      private readonly ILogger<AttributeValueTypesController> _logger;
      private readonly ProductManagementContext _dbContext;

      public AttributeValueTypesController(ILogger<AttributeValueTypesController> logger, ProductManagementContext dbContext)
      {
         _logger = logger;
         _dbContext = dbContext;
      }

      [HttpGet]
      public async Task<IEnumerable<AttributeValueType>> Get()
      {
         ClaimsPrincipal user = HttpContext.User;

         return await _dbContext.AttributeValueType
               //.Where(filterExpression)
               .AsNoTracking()
               .ToArrayAsync();
      }
   }
}
