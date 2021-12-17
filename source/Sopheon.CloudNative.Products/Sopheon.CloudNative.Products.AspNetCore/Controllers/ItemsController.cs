using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
         int newValue = itemFromRequest.EnumCollectionAttributeValues.Single(ecav => ecav.AttributeId == STATUS).Value.Single().EnumAttributeOptionId;
         EnumCollectionAttributeValue toDoItems = itemFromDB.EnumCollectionAttributeValues.Single(ecav => ecav.AttributeId == STATUS);
         toDoItems.Value = new List<EnumAttributeOptionValue> { new EnumAttributeOptionValue { EnumAttributeOptionId = newValue } };

         _ = await _dbContext.SaveChangesAsync();

         return Ok(_mapper.Map<ProductItemDto>(itemFromDB));
      }
   }
}
