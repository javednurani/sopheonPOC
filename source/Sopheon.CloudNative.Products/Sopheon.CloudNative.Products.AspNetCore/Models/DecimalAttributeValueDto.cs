using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class DecimalAttributeValueDto : IAttributeValueDto<decimal?>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public AttributeDto Attribute { get; set; }

      public decimal? Value { get; set; }
   }
}
