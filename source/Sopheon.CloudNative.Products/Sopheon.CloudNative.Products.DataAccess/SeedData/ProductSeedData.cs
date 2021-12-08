using System;
using System.Collections.Generic;
using System.Linq;
using Sopheon.CloudNative.Products.Domain;
using Sopheon.CloudNative.Products.Domain.Attributes.Enum;
using Sopheon.CloudNative.Products.Domain.Attributes.Int32;
using Sopheon.CloudNative.Products.Domain.Attributes.String;
using Sopheon.CloudNative.Products.Domain.Attributes.UtcDateTime;

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

      private static readonly List<Domain.Attribute> _defaultAttributes = new List<Domain.Attribute>();

      static ProductSeedData()
      {
         _systemAttributeDataTypes = DefineDefaultAttributeDataTypes();
         _systemManagedProductItemTypes = DefineSystemManagedProductItemTypes();
         _systemManagedStatuses = DefineSystemManagedStatuses();

         DefineDefaultAttributes();
      }

      public static TEntity[] GetDefaultAttributes<TEntity>() where TEntity : Domain.Attribute
      {
         return _defaultAttributes.OfType<TEntity>().ToArray();
      }

      private static AttributeDataType[] DefineDefaultAttributeDataTypes()
      {
         return Enum.GetValues(typeof(AttributeDataTypes))
                     .Cast<AttributeDataTypes>()
                     .Select(e => new AttributeDataType()
                     {
                        AttributeDataTypeId = (int)e,
                        Name = e.ToString()
                     }).ToArray();
      }

      private static Status[] DefineSystemManagedStatuses()
      {
         return Enum.GetValues(typeof(SystemManagedStatusIds))
                        .Cast<SystemManagedStatusIds>()
                        .Select(e => new Status()
                        {
                           Id = (int)e,
                           Name = e.ToString()
                        }).ToArray();
      }

      private static ProductItemType[] DefineSystemManagedProductItemTypes()
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

      private static void AddDefaultAttributes<TAttribute>(IEnumerable<TAttribute> attributes) where TAttribute : Domain.Attribute
      {
         foreach (TAttribute attribute in attributes)
         {
            _defaultAttributes.Add(attribute);
         }
      }

      private static void DefineDefaultAttributes()
      {
         // TODO, Status will be an EnumAttribute, NOT EnumCollectionAttribute
         List<EnumCollectionAttribute> enumCollectionAttributes = new List<EnumCollectionAttribute>
         {
            new EnumCollectionAttribute
            {
               AttributeId = -4,
               Name = "Status",
               ShortName = "STATUS",
               EnumAttributeOptions = new List<EnumAttributeOption>
               {
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -1,
                     Name = "Not Started"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -2,
                     Name = "In Progress"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -3,
                     Name = "Assigned"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -4,
                     Name = "Complete"
                  },
               }
            }
         };

         enumCollectionAttributes.ForEach(attribute =>
         {
            foreach (EnumAttributeOption option in attribute.EnumAttributeOptions)
            {
               option.AttributeId = attribute.AttributeId;
            }
         });

         List<Domain.Attribute> otherDefaultAttributes = new List<Domain.Attribute>
         {
            new Int32Attribute
            {
               AttributeId = -1,
               Name = "Industry",
               ShortName = "IND"
            },
            new StringAttribute
            {
               AttributeId = -2,
               Name = "Notes",
               ShortName = "NOTES"
            },
            new UtcDateTimeAttribute
            {
               AttributeId = -3,
               Name = "Due Date",
               ShortName = "DUE"
            }
         };

         AddDefaultAttributes(otherDefaultAttributes);
         AddDefaultAttributes(enumCollectionAttributes);
      }
   }
}
