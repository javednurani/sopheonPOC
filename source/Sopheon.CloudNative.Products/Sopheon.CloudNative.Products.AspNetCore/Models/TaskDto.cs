using System;

namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class TaskDto
   {
      public int Id { get; set; }

      // TODO, may not need ProductId in DTO, if we have productKey from request
      //public int ProductId { get; set; }

      public string Name { get; set; }

      public string Notes { get; set; }

      public int Status { get; set; }

      public DateTime DueDate { get; set; }
   }
}
