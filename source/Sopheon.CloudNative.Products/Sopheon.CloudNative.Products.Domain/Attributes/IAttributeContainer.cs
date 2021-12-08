using Sopheon.CloudNative.Products.Domain.Attributes.Decimal;
using Sopheon.CloudNative.Products.Domain.Attributes.Int32;
using Sopheon.CloudNative.Products.Domain.Attributes.Money;
using Sopheon.CloudNative.Products.Domain.Attributes.String;
using Sopheon.CloudNative.Products.Domain.Attributes.UtcDateTime;

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
