using System.Collections.Generic;

namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class ProductItemDto
   {
      public int Id { get; set; }

      public string Name { get; set; }

      public int ProductItemTypeId { get; set; }

      /// <summary>
      /// Navigation Property
      /// </summary>
      public ProductItemTypeDto ProductItemType { get; set; }

      //public Rank Rank { get; set; }

      public List<Int32AttributeValueDto> IntAttributeValues { get; set; }

      public List<StringAttributeValueDto> StringAttributeValues { get; set; }

      public List<DecimalAttributeValueDto> DecimalAttributeValues { get; set; }

      public List<UtcDateTimeAttributeValueDto> UtcDateTimeAttributeValues { get; set; }

      public List<MoneyAttributeValueDto> MoneyAttributeValues { get; set; }
   }
}
