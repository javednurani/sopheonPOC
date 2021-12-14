namespace Sopheon.CloudNative.Products.AspNetCore.Models
{
   public class EnumAttributeValueDto : IAttributeValueDto<EnumAttributeOptionDto>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public AttributeDto Attribute { get; set; }

      public EnumAttributeOptionDto Value { get; set; }
   }
}
