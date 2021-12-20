

namespace Sopheon.CloudNative.Products.Domain.Attributes.Decimal
{
   public class DecimalAttributeValue : IAttributeValue<decimal?>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public Attribute Attribute { get; set; }

      public decimal? Value { get; set; }
   }
}
