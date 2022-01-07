using System;
namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class TaskChangeEventDto
   {
      public EntityChangeEventTypesDto EntityChangeEventType { get; set; }
      public TaskDeltaDto PreValue { get; set; }
      public TaskDeltaDto PostValue { get; set; }
      public DateTime Timestamp { get; set; }
   }
}