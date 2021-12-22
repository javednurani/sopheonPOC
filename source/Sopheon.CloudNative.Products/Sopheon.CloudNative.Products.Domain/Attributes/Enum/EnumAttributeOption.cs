

namespace Sopheon.CloudNative.Products.Domain.Attributes.Enum
{
   public class EnumAttributeOption
   {
      public int  EnumAttributeOptionId { get; set; }

      public string Name { get; set; }

      public int AttributeId { get; set; }

      public EnumCollectionAttribute EnumCollectionAttribute { get; set; }
   }
}
