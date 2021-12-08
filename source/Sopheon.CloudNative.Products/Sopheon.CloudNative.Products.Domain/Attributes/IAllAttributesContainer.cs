using Sopheon.CloudNative.Products.Domain.Attributes.Decimal;
using Sopheon.CloudNative.Products.Domain.Attributes.Enum;
using Sopheon.CloudNative.Products.Domain.Attributes.Int32;
using Sopheon.CloudNative.Products.Domain.Attributes.Money;
using Sopheon.CloudNative.Products.Domain.Attributes.String;
using Sopheon.CloudNative.Products.Domain.Attributes.UtcDateTime;

namespace Sopheon.CloudNative.Products.Domain
{
   public interface IAllAttributesContainer :
      IDecimalAttributeContainer,
      IEnumCollectionAttributeContainer,
      IInt32AttributeContainer,
      IMoneyAttributeContainer,
      IStringAttributeContainer,
      IUtcDateTimeAttributeContainer
   {
   }
}
