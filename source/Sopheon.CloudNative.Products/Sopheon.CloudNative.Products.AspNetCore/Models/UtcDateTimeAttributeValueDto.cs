using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class UtcDateTimeAttributeValueDto : IAttributeValueDto<DateTime?>
   {
      public int AttributeId { get; set; }

      public AttributeDto Attribute { get; set; }

      public DateTime? Value { get; set; }
   }
}
