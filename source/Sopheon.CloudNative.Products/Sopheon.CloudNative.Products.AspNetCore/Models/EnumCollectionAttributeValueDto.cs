using System.Collections.Generic;

namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class EnumCollectionAttributeValueDto : IAttributeValueDto<List<EnumAttributeOptionValueDto>>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public AttributeDto Attribute { get; set; }

      public List<EnumAttributeOptionValueDto> Value { get; set; }
   }
}
