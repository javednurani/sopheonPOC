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
   public class ProductsController : EnvironmentScopedControllerBase
   {
      private readonly ILogger<ProductsController> _logger;
      private readonly ProductManagementContext _dbContext;
      private readonly IMapper _mapper;

      public ProductsController(ILogger<ProductsController> logger,
         ProductManagementContext dbContext,
         IMapper mapper)
      {
         _logger = logger;
         _dbContext = dbContext;
         _mapper = mapper;
      }

      [ProducesResponseType(StatusCodes.Status200OK)]
      [HttpGet]
      public async Task<IEnumerable<ProductDto>> Get()
      {
         _logger.LogInformation("ProductsController::Get");
         var query = _dbContext.Products
               .AsNoTracking()
               .ProjectTo<ProductDto>(_mapper.ConfigurationProvider);

         return await query.ToArrayAsync();
      }

      [HttpGet("{key}")]
      public async Task<IActionResult> GetByKey(string key)
      {
         var product = await _dbContext.Products
             .Include(p => p.Goals)
             .Include(p => p.KeyPerformanceIndicators)
             .AsNoTracking()
             .SingleOrDefaultAsync(p => p.Key == key);

         if (product == null) { return NotFound(); }

         return Ok(_mapper.Map<ProductDto>(product));
      }

      [HttpPatch("{key}")]
      public async Task<IActionResult> Patch(string key, [FromBody] JsonPatchDocument<ProductPatchDto> patchDocument)
      {
         if (patchDocument.Operations.Count == 0)
         {
            return NoContent();
         }

         Product productFromDatabase = await _dbContext.Products.SingleOrDefaultAsync(p => p.Key == key);

         if (productFromDatabase == null)
         {
            return NotFound();
         }

         ProductPatchDto productDto = _mapper.Map<ProductPatchDto>(productFromDatabase);

         patchDocument.ApplyTo(productDto); //Apply the patch to that DTO. 
         _mapper.Map(productDto, productFromDatabase); //Use automapper to map the DTO back ontop of the database object. 

         await _dbContext.SaveChangesAsync();

         // fetch updated product to ensure related entity Id's are populated (eg Attributes, KPIs, Goals)
         var updatedProduct = await _dbContext.Products
            .Include(p => p.Items)
            .ThenInclude(i => i.StringAttributeValues)
            .Include(p => p.Items)
            .ThenInclude(i => i.UtcDateTimeAttributeValues)
            .Include(p => p.Items)
            .ThenInclude(i => i.EnumAttributeValues)
            .Include(p => p.Goals)
            .Include(p => p.KeyPerformanceIndicators)
            .ThenInclude(kpi => kpi.Attribute)
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Key == key);

         return Ok(_mapper.Map<ProductDto>(updatedProduct));
      }

      [HttpPost]
      public async Task<IActionResult> PostAsync([FromBody] ProductPostDto productPostDto)
      {
         Product product = _mapper.Map<Product>(productPostDto);
         product.Key = Guid.NewGuid().ToString();

         await _dbContext.Products.AddAsync(product);

         await _dbContext.SaveChangesAsync();

         ProductDto resultProduct = _mapper.Map<ProductDto>(product);

         return CreatedAtAction(nameof(GetByKey), new { EnvironmentId = Request.RouteValues["EnvironmentId"], key = resultProduct.Key }, resultProduct);
      }
   }
}
