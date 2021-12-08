﻿#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Sopheon.CloudNative.Products.Domain.Attributes.Decimal;
using Sopheon.CloudNative.Products.Domain.Attributes.Int32;
using Sopheon.CloudNative.Products.Domain.Attributes.Money;
using Sopheon.CloudNative.Products.Domain.Attributes.String;
using Sopheon.CloudNative.Products.Domain.Attributes.UtcDateTime;

namespace Sopheon.CloudNative.Products.Domain
{
   public class ProductItem : IRankedEntity, IAttributeContainer
   {
      public int Id { get; set; }

      public string Name { get; set; }

      public int ProductItemTypeId { get; set; }

      /// <summary>
      /// Navigation Property
      /// </summary>
      public ProductItemType ProductItemType { get; set; }

      public Rank Rank { get; set; }

      public List<Int32AttributeValue> IntAttributeValues { get; set; }

      public List<StringAttributeValue> StringAttributeValues { get; set; }

      public List<DecimalAttributeValue> DecimalAttributeValues { get; set; }

      public List<UtcDateTimeAttributeValue> UtcDateTimeAttributeValues { get; set; }

      public List<MoneyAttributeValue> MoneyAttributeValues { get; set; }
   }
}
