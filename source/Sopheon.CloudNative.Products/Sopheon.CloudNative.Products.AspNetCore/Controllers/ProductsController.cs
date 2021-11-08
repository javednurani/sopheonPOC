﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Products.AspNetCore.Models;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.AspNetCore.Controllers
{
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
         var query = _dbContext.Products
               .AsNoTracking()
               .ProjectTo<ProductDto>(_mapper.ConfigurationProvider);

         return await query.ToArrayAsync();
      }

      [HttpGet("{key}/Items")]
      public async Task<IActionResult> GetItems(string key)
      {
         var query = _dbContext.Products
             .AsNoTracking()
             .Where(p => p.Key == key)
             .Select(p => p.Items)
             .ProjectTo<ProductItemDto>(_mapper.ConfigurationProvider);

         var results = await query.ToArrayAsync();
         return Ok(results);
      }

      [HttpPatch("{key}")]
      public async Task<IActionResult> Patch(string key, [FromBody] JsonPatchDocument<ProductPatchDto> patchDocument)
      {
         if (patchDocument.Operations.Count == 0)
         {
            return NoContent();
         }

         var test = await _dbContext.Products.ToListAsync();
         Product productFromDatabase = await _dbContext.Products.SingleOrDefaultAsync(p => p.Key == key);
         if (productFromDatabase == null)
         {
            return NotFound();
         }

         ProductPatchDto productDto = _mapper.Map<ProductPatchDto>(productFromDatabase);

         patchDocument.ApplyTo(productDto); //Apply the patch to that DTO. 
         _mapper.Map(productDto, productFromDatabase); //Use automapper to map the DTO back ontop of the database object. 

         await _dbContext.SaveChangesAsync();

         return NoContent();
      }
   }
}