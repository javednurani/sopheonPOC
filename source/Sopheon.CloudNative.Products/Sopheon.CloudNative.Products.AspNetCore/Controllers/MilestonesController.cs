using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Products.AspNetCore.Filters;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.AspNetCore.Controllers
{
   [TypeFilter(typeof(GeneralExceptionFilter))]

   [Route("Environments/{EnvironmentId}/Products/{productKey}/[controller]")]
   public class MilestonesController : EnvironmentScopedControllerBase
   {
      private readonly ILogger<MilestonesController> _logger;
      private readonly ProductManagementContext _dbContext;
      private readonly IMapper _mapper;

      public MilestonesController(ILogger<MilestonesController> logger,
         ProductManagementContext dbContext,
         IMapper mapper)
      {
         _logger = logger;
         _dbContext = dbContext;
         _mapper = mapper;
      }
   }
}
