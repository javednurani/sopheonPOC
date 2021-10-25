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
   public class StatusController : EnvironmentScopedControllerBase
   {
      private readonly ILogger<StatusController> _logger;
      private readonly ProductManagementContext _dbContext;
      private readonly IMapper _mapper;

      public StatusController(ILogger<StatusController> logger,
         ProductManagementContext dbContext,
         IMapper mapper)
      {
         _logger = logger;
         _dbContext = dbContext;
         _mapper = mapper;
      }

      [HttpGet]
      public async Task<IEnumerable<StatusDto>> Get()
      {
         //ClaimsPrincipal user = HttpContext.User;

         return await _dbContext.Status
               //.Where(filterExpression)
               .AsNoTracking()
               .ProjectTo<StatusDto>(_mapper.ConfigurationProvider)
               .ToArrayAsync();
      }
   }
}
