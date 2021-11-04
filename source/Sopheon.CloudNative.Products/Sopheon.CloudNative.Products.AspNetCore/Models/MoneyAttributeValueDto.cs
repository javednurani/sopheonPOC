using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class MoneyAttributeValueDto : IAttributeValueDto<MoneyValueDto>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public AttributeDto Attribute { get; set; }

      public MoneyValueDto Value { get; set; }
   }
}
