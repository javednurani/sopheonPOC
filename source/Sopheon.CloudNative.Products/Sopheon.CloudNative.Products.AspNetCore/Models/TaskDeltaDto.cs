using System;
namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class TaskDeltaDto
   {
      public string Name { get; set; }
      public string Notes { get; set; }
      public DateTime DueDate { get; set; }
      public TaskStatusDto Status { get; set; }
   }
}