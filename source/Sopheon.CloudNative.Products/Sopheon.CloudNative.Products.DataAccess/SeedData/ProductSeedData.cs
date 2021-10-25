using System;
using System.Linq;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.DataAccess.SeedData
{
   public static class ProductSeedData
   {
      private static readonly AttributeValueType[] _systemAttributeValueTypes;
      public static AttributeValueType[] SystemAttributeValueTypes => _systemAttributeValueTypes.ToArray();

      private static readonly ProductItemType[] _systemManagedProductItemTypes;
      public static ProductItemType[] SystemManagedProductItemTypes => _systemManagedProductItemTypes.ToArray();

      private static readonly Status[] _systemManagedStatuses;
      public static Status[] SystemManagedStatuses => _systemManagedStatuses.ToArray();

      private static readonly Domain.Attribute[] _defaultAttributes;
      public static Domain.Attribute[] DefaultAttributes => _defaultAttributes.ToArray();

      static ProductSeedData()
      {
         _systemAttributeValueTypes = GetDefaultAttributeValueTypes();
         _systemManagedProductItemTypes = GetSystemManagedProductItemTypes();
         _systemManagedStatuses = GetSystemManagedStatuses();
         _defaultAttributes = GetDefaultAttributes();
      }

      private static AttributeValueType[] GetDefaultAttributeValueTypes()
      {
         return Enum.GetValues(typeof(AttributeValueTypes))
                     .Cast<AttributeValueTypes>()
                     .Select(e => new AttributeValueType()
                     {
                        AttributeValueTypeId = (int)e,
                        Name = e.ToString()
                     }).ToArray();
      }

      private static Status[] GetSystemManagedStatuses()
      {
         return Enum.GetValues(typeof(SystemManagedStatusIds))
                        .Cast<SystemManagedStatusIds>()
                        .Select(e => new Status()
                        {
                           Id = (int)e,
                           Name = e.ToString()
                        }).ToArray();
      }

      private static ProductItemType[] GetSystemManagedProductItemTypes()
      {
         return Enum.GetValues(typeof(SystemManagedProductItemTypeIds))
                        .Cast<SystemManagedProductItemTypeIds>()
                        .Select(e => new ProductItemType()
                        {
                           Id = (int)e,
                           Name = e.ToString()
                        })
                        .ToArray();
      }

      private static Domain.Attribute[] GetDefaultAttributes()
      {
         return new[]
            {
               new Domain.Attribute()
               {
                  AttributeId = -1,
                  Name = "Net Present Value",
                  ShortName = "NPV",
                  AttributeValueTypeId = (int)AttributeValueTypes.Money
               },
               new Domain.Attribute()
               {
                  AttributeId = -2,
                  Name = "Industry",
                  AttributeValueTypeId = (int)AttributeValueTypes.String
               },
               new Domain.Attribute()
               {
                  AttributeId = -3,
                  Name = "Risk Score",
                  AttributeValueTypeId = (int)AttributeValueTypes.Int32
               },
               new Domain.Attribute()
               {
                  AttributeId = -4,
                  Name = "Initial Release Date",
                  AttributeValueTypeId = (int)AttributeValueTypes.UtcDateTime
               }
            };
      }
   }
}
