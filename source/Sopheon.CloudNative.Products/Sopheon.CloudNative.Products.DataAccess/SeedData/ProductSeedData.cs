using System;
using System.Linq;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.DataAccess.SeedData
{
   public static class ProductSeedData
   {
      private static readonly AttributeDataType[] _systemAttributeDataTypes;
      public static AttributeDataType[] SystemAttributeDataTypes => _systemAttributeDataTypes.ToArray();

      private static readonly ProductItemType[] _systemManagedProductItemTypes;
      public static ProductItemType[] SystemManagedProductItemTypes => _systemManagedProductItemTypes.ToArray();

      private static readonly Status[] _systemManagedStatuses;
      public static Status[] SystemManagedStatuses => _systemManagedStatuses.ToArray();

      private static readonly Domain.Attribute[] _defaultAttributes;
      public static Domain.Attribute[] DefaultAttributes => _defaultAttributes.ToArray();

      static ProductSeedData()
      {
         _systemAttributeDataTypes = GetDefaultAttributeDataTypes();
         _systemManagedProductItemTypes = GetSystemManagedProductItemTypes();
         _systemManagedStatuses = GetSystemManagedStatuses();
         _defaultAttributes = GetDefaultAttributes();
      }

      private static AttributeDataType[] GetDefaultAttributeDataTypes()
      {
         return Enum.GetValues(typeof(AttributeDataTypes))
                     .Cast<AttributeDataTypes>()
                     .Select(e => new AttributeDataType()
                     {
                        AttributeDataTypeId = (int)e,
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
                  Name = "Industry",
                  ShortName = "IND",
                  AttributeDataTypeId = (int)AttributeDataTypes.Int32
               },
               new Domain.Attribute()
               {
                  AttributeId = -2,
                  Name = "Notes",
                  ShortName = "NOTES",
                  AttributeDataTypeId = (int)AttributeDataTypes.String
               },
               new Domain.Attribute()
               {
                  AttributeId = -3,
                  Name = "Due Date",
                  ShortName = "DUE",
                  AttributeDataTypeId = (int)AttributeDataTypes.UtcDateTime
               }
            };
      }
   }
}
