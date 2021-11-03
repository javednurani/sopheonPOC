using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class Int32AttributeValueDto : IAttributeValueDto<int?>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public AttributeDto Attribute { get; set; }

      public int? Value { get; set; }
   }
}
