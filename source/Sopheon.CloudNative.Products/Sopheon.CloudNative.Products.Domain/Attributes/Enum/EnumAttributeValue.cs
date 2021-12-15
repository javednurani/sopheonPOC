namespace Sopheon.CloudNative.Products.Domain.Attributes.Enum
{
   public class EnumAttributeValue : IAttributeValue<EnumAttributeOption>
   {
      public int AttributeId { get; set; }

      public Attribute Attribute { get; set; }

      public EnumAttributeOption Value { get; set; }

      public int EnumAttributeOptionId { get; set; }
   }
}
