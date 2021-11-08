using System.Collections.Generic;

namespace Sopheon.CloudNative.Products.Domain
{
   public interface IAttributeContainer
   {
      List<Int32AttributeValue> IntAttributeValues { get; set; }

      List<StringAttributeValue> StringAttributeValues { get; set; }

      List<DecimalAttributeValue> DecimalAttributeValues { get; set; }

      List<UtcDateTimeAttributeValue> UtcDateTimeAttributeValues { get; set; }

      List<MoneyAttributeValue> MoneyAttributeValues { get; set; }
   }
}
