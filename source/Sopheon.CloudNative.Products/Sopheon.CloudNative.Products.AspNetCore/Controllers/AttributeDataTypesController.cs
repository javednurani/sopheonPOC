using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.AspNetCore.Controllers
{
   public class AttributeDataTypesController : EnvironmentScopedControllerBase
   {
      private readonly ILogger<AttributeDataTypesController> _logger;
      private readonly ProductManagementContext _dbContext;

      public AttributeDataTypesController(ILogger<AttributeDataTypesController> logger, ProductManagementContext dbContext)
      {
         _logger = logger;
         _dbContext = dbContext;
      }

      [HttpGet]
      public async Task<IEnumerable<AttributeDataType>> Get()
      {
         ClaimsPrincipal user = HttpContext.User;

         return await _dbContext.AttributeDataType
               //.Where(filterExpression)
               .AsNoTracking()
               .ToArrayAsync();
      }
   }
}
