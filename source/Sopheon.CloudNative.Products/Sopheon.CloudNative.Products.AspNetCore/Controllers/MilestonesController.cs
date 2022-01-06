using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Products.AspNetCore.Filters;
using Sopheon.CloudNative.Products.AspNetCore.Models;
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


      [HttpPost]
      public async Task<IActionResult> PostMilestone(string productKey, [FromBody] MilestoneDto milestoneDto)
      {
         Product product = await _dbContext.Products
             .SingleOrDefaultAsync(p => p.Key == productKey);
         if (product == null) { return NotFound(); }

         Milestone milestone = _mapper.Map<Milestone>(milestoneDto);
         milestone.ProductId = product.Id;

         _dbContext.Milestones.Add(milestone);
         await _dbContext.SaveChangesAsync();

         return Created("TODO-implement Get single Milestone endpoint", _mapper.Map<MilestoneDto>(milestone));
      }
   }
}
