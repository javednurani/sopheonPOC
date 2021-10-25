using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Products.AspNetCore.Models;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.AspNetCore.Controllers
{
   public class ProductItemTypeController : EnvironmentScopedControllerBase
   {
      private readonly ILogger<ProductItemTypeController> _logger;
      private readonly ProductManagementContext _dbContext;
      private readonly IMapper _mapper;

      public ProductItemTypeController(ILogger<ProductItemTypeController> logger,
         ProductManagementContext dbContext,
         IMapper mapper)
      {
         _logger = logger;
         _dbContext = dbContext;
         _mapper = mapper;
      }

      [HttpGet]
      public async Task<IEnumerable<ProductItemTypeDto>> Get()
      {
         //ClaimsPrincipal user = HttpContext.User;

         return await _dbContext.ProductItemType
               //.Where(filterExpression)
               .AsNoTracking()
               .ProjectTo<ProductItemTypeDto>(_mapper.ConfigurationProvider)
               .ToArrayAsync();
      }
   }
}
