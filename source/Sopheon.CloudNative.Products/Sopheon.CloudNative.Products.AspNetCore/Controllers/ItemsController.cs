using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Products.AspNetCore.Filters;
using Sopheon.CloudNative.Products.AspNetCore.Models;
using Sopheon.CloudNative.Products.Domain;
using Sopheon.CloudNative.Products.Domain.Attributes.Enum;

namespace Sopheon.CloudNative.Products.AspNetCore.Controllers
{
   [TypeFilter(typeof(GeneralExceptionFilter))]

   [Route("Environments/{EnvironmentId}/Products/{productKey}/[controller]")]
   public class ItemsController : EnvironmentScopedControllerBase
   {
      private readonly int STATUS = -4; // TODO: do we have enum for these?  (SPM.Attribute)
      private readonly ILogger<ProductsController> _logger;
      private readonly ProductManagementContext _dbContext;
      private readonly IMapper _mapper;

      public ItemsController(ILogger<ProductsController> logger,
         ProductManagementContext dbContext,
         IMapper mapper)
      {
         _logger = logger;
         _dbContext = dbContext;
         _mapper = mapper;
      }

      [HttpGet]
      public async Task<IActionResult> GetItems(string productKey)
      {
         var query = _dbContext.Products
             .AsNoTracking()
             .Where(p => p.Key == productKey)
             .Select(p => p.Items)
             .ProjectTo<ProductItemDto>(_mapper.ConfigurationProvider);

         var results = await query.ToArrayAsync();
         return Ok(results);
      }

      
      // INFO, this endpoint was added in Cloud-2183 story to support rapid development of adding ProductItems
      // the ProductsController::Patch endpoint also supports this, and React SPA infrastructure for API calls is more in parity with the Patch endpoint
      // For purpose of Cloud-2183, may not need to use this endpoint. But it may be valuable in the future
      [HttpPost]
      public async Task<IActionResult> PostItems(string productKey, [FromBody] ProductItemDto itemDto) // TODO, PostItem vs PostItems, single Dto vs collection of Dto's in request...
      {
         Product product = await _dbContext.Products
             .Include(p => p.Items)
             .SingleOrDefaultAsync(p => p.Key == productKey);

         if (product == null)
         {
            return NotFound();
         }

         // TODO, validate Dto?
         ProductItem item = _mapper.Map<ProductItem>(itemDto);
         product.Items.Add(item);

         await _dbContext.SaveChangesAsync();

         return Ok(); // TODO, return 201 Created w/ a Response Body including new Id(s)
      }
      

      [HttpPut("{itemId}")]
      public async Task<IActionResult> PutItem(string productKey, int itemId, [FromBody] ProductItemDto productItemDto)
      {
         Product product = await _dbContext.Products
            .Include(p => p.Items)
            .SingleOrDefaultAsync(p => p.Key == productKey);
         if (product == null) { return NotFound(); }

         ProductItem itemFromDB = product.Items.SingleOrDefault(i => i.Id == itemId);
         if (itemFromDB == null) { return NotFound(); }

         ProductItem itemFromRequest = _mapper.Map<ProductItem>(productItemDto);

         // update entity
         // TODO: currently only setting the value for the single ToDoItem
         int newValue = itemFromRequest.EnumAttributeValues.Single(ecav => ecav.AttributeId == STATUS).EnumAttributeOptionId;
         EnumAttributeValue toDoItem = itemFromDB.EnumAttributeValues.Single(ecav => ecav.AttributeId == STATUS);
         toDoItem.EnumAttributeOptionId = newValue;

         _ = await _dbContext.SaveChangesAsync();

         return Ok(_mapper.Map<ProductItemDto>(itemFromDB));
      }
   }
}
