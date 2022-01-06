using System;

namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class MilestoneDto
   {
      public int Id { get; set; }

      public string Name { get; set; }

      public string Notes { get; set; }

      public DateTime? Date { get; set; }
   }
}
