#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Sopheon.CloudNative.Products.Domain
{
   public class UtcDateTimeAttributeValue : IAttributeValue<DateTime?>
   {
      public int AttributeId { get; set; }

      public Attribute Attribute { get; set; }

      public DateTime? Value { get; set; }
   }
}
