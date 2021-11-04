using System;
using System.Collections.Generic;

namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public interface IAttributeValueDto<TValueType>
   {
      int AttributeId { get; set; }

      AttributeDto Attribute { get; set; }

      TValueType Value { get; set; }
   }
}
