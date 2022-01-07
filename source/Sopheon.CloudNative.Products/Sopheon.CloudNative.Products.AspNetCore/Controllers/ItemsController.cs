using System;
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
using Sopheon.CloudNative.Products.Domain.Attributes.String;
using Sopheon.CloudNative.Products.Domain.Attributes.UtcDateTime;

namespace Sopheon.CloudNative.Products.AspNetCore.Controllers
{
   [TypeFilter(typeof(GeneralExceptionFilter))]

   [Route("Environments/{EnvironmentId}/Products/{productKey}/[controller]")]
   public class ItemsController : EnvironmentScopedControllerBase
   {
      private readonly int STATUS = -4; // TODO: do we have enum for these?  (SPM.Attribute)
      private readonly int DATE = -3;
      private readonly int NOTES = -2;

      private readonly ILogger<ItemsController> _logger;
      private readonly ProductManagementContext _dbContext;
      private readonly IMapper _mapper;

      public ItemsController(ILogger<ItemsController> logger,
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

         DateTime? newDate = itemFromRequest.UtcDateTimeAttributeValues.Single(ecav => ecav.AttributeId == DATE).Value;
         UtcDateTimeAttributeValue toDoDate = itemFromDB.UtcDateTimeAttributeValues.Single(ecav => ecav.AttributeId == DATE);
         toDoDate.Value = newDate;

         string newNote = itemFromRequest.StringAttributeValues.Single(ecav => ecav.AttributeId == NOTES).Value;
         StringAttributeValue toDoItemNote = itemFromDB.StringAttributeValues.Single(ecav => ecav.AttributeId == NOTES);
         toDoItemNote.Value = newNote;

         itemFromDB.Name = itemFromRequest.Name;
         _ = await _dbContext.SaveChangesAsync();

         return Ok(_mapper.Map<ProductItemDto>(itemFromDB));
      }
   }
}
