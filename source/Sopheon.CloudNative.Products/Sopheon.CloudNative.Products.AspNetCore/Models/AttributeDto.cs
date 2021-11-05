namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class AttributeDto
   {
      public int AttributeId { get; set; }

      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeValueTypeId { get; set; }

      /// <summary>
      /// Navigation Property
      /// </summary>
      public AttributeValueTypeDto AttributeValueType { get; set; }

      public string Name { get; set; }

      public string ShortName { get; set; }

      public bool IsSystem()
      {
         return AttributeId < 0;
      }
   }
}
