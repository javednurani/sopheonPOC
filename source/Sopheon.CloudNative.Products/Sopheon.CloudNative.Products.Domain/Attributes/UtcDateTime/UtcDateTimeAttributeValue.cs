

namespace Sopheon.CloudNative.Products.Domain.Attributes.UtcDateTime
{
   public class UtcDateTimeAttributeValue : IAttributeValue<DateTime?>
   {
      public int AttributeId { get; set; }

      public Attribute Attribute { get; set; }

      public DateTime? Value { get; set; }
   }
}
