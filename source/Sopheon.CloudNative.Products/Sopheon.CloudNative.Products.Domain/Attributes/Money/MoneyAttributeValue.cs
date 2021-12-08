

namespace Sopheon.CloudNative.Products.Domain.Attributes.Money
{
   public class MoneyAttributeValue : IAttributeValue<MoneyValue>
   {
      /// <summary>
      /// Foreign Key to Attribute Type
      /// </summary>
      public int AttributeId { get; set; }

      public Attribute Attribute { get; set; }

      public MoneyValue Value { get; set; }
   }
}
