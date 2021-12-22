

namespace Sopheon.CloudNative.Products.Domain.Attributes.Enum
{
   public class EnumCollectionAttributeValue : IAttributeValue<List<EnumAttributeOptionValue>>
   {
      public int AttributeId { get; set; }

      public Attribute Attribute { get; set; }

      public List<EnumAttributeOptionValue> Value { get; set; }
   }
}
