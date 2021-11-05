namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class StringAttributeValueDto : IAttributeValueDto<string>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public AttributeDto Attribute { get; set; }

      public string Value { get; set; }
   }
}
