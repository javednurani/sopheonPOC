using System;

namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class TaskHistoryItemDto
   {
      public string FieldName { get; set; }

      public DateTime TimeStamp { get; set; }

      // changed from / "old value"
      public string Value { get; set; }
   }
}
