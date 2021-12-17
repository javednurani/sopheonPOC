using System;
using System.Collections.Generic;
using System.Linq;
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
         // TODO, fully implement this endpoint. currently only returning Int32Attributes
         return await _dbContext.Int32Attributes
               .AsNoTracking()
               .Cast<Domain.Attribute>()
               .ToArrayAsync();
      }
   }
}
