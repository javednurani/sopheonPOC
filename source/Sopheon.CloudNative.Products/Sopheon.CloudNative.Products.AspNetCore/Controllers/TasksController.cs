using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Products.AspNetCore.Filters;
using Sopheon.CloudNative.Products.AspNetCore.Models;
using Sopheon.CloudNative.Products.Domain;
using Task = Sopheon.CloudNative.Products.Domain.Task;

namespace Sopheon.CloudNative.Products.AspNetCore.Controllers
{
   [TypeFilter(typeof(GeneralExceptionFilter))]

   [Route("Environments/{EnvironmentId}/Products/{productKey}/[controller]")]
   public class TasksController : EnvironmentScopedControllerBase
   {
      private readonly ILogger<TasksController> _logger;
      private readonly ProductManagementContext _dbContext;
      private readonly IMapper _mapper;

      public TasksController(ILogger<TasksController> logger,
         ProductManagementContext dbContext,
         IMapper mapper)
      {
         _logger = logger;
         _dbContext = dbContext;
         _mapper = mapper;
      }

      [HttpGet("{taskId}/History")]
      public async Task<IActionResult> GetTaskHistory(string productKey, int taskId)
      {
         Product product = await _dbContext.Products
             .Include(p => p.Tasks)
             .SingleOrDefaultAsync(p => p.Key == productKey);

         if (product == null)
         {
            return NotFound();
         }

         Task task = product.Tasks.SingleOrDefault(t => t.Id == taskId);

         if (task == null)
         {
            return NotFound();
         }

         // TODO, implement History using dbContext.Tasks.TemporalAll
         // build an array of TaskHistoryItemDto's to return

         return Ok();
      }

      [HttpPost]
      public async Task<IActionResult> PostTask(string productKey, [FromBody] TaskDto taskDto)
      {
         Product product = await _dbContext.Products
             .SingleOrDefaultAsync(p => p.Key == productKey);

         if (product == null)
         {
            return NotFound();
         }

         Task task = _mapper.Map<Task>(taskDto);
         task.ProductId = product.Id;

         _dbContext.Tasks.Add(task);
         await _dbContext.SaveChangesAsync();

         return Created("TODO-implement Get single Task endpoint", _mapper.Map<TaskDto>(task));
      }
      

      [HttpPut("{taskId}")]
      public async Task<IActionResult> PutTask(string productKey, int taskId, [FromBody] TaskDto taskDto)
      {
         Product product = await _dbContext.Products
            .SingleOrDefaultAsync(p => p.Key == productKey);

         if (product == null) { return NotFound(); }

         Task taskFromDB = await _dbContext.Tasks.SingleOrDefaultAsync(t => t.Id == taskId && t.ProductId == product.Id);
         if (taskFromDB == null) { return NotFound(); }

         Task taskFromRequest = _mapper.Map<Task>(taskDto);

         // update entity
         taskFromDB.Name = taskFromRequest.Name;
         taskFromDB.Notes = taskFromRequest.Notes;
         taskFromDB.DueDate = taskFromRequest.DueDate;
         taskFromDB.Status = taskFromRequest.Status;

         _ = await _dbContext.SaveChangesAsync();

         return Ok(_mapper.Map<TaskDto>(taskFromDB));
      }

      // INFO, the Patch endpoint below is not tested.
      // For partial updates of Task (eg, [un]checking of "Complete/Incomplete" checkbox from ToDoList)
      // ... easiest way forward is to pass the complete Task object into the Put method.
      // ... ideally we will use Patch endpoints for partial updates (eg, update ONLY Status of a Task)

      //[HttpPatch("{taskId}")]
      //public async Task<IActionResult> PatchTask(string productKey, int taskId, [FromBody] JsonPatchDocument<TaskDto> patchDocument)
      //{
      //   if (patchDocument.Operations.Count == 0)
      //   {
      //      return NoContent();
      //   }

      //   Product productFromDatabase = await _dbContext.Products.SingleOrDefaultAsync(p => p.Key == productKey);

      //   if (productFromDatabase == null)
      //   {
      //      return NotFound();
      //   }

      //   Task taskFromDatabase = await _dbContext.Tasks.SingleOrDefaultAsync(t => t.Id == taskId && t.ProductId == productFromDatabase.Id);

      //   TaskDto taskDto = _mapper.Map<TaskDto>(taskFromDatabase);
      //   patchDocument.ApplyTo(taskDto);  //Apply the patch to that DTO. 
      //   _mapper.Map(taskDto, taskFromDatabase);  //Use automapper to map the DTO back ontop of the database object. 

      //   await _dbContext.SaveChangesAsync();

      //   return Ok(_mapper.Map<TaskDto>(taskFromDatabase));
      //}
   }
}
