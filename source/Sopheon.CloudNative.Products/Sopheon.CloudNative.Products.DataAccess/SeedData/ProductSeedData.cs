using System;
using System.Collections.Generic;
using System.Linq;
using Sopheon.CloudNative.Products.Domain;
using Sopheon.CloudNative.Products.Domain.Attributes.Enum;
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
         // EnumAttributes = SINGLE-SELECT of Enum values
         List<EnumAttribute> enumAttributes = new List<EnumAttribute>
         {
            new EnumAttribute
            {
               AttributeId = -4,
               AttributeDataTypeId = (int)AttributeDataTypes.Enum,
               Name = "Status",
               ShortName = "STATUS",
               EnumAttributeOptions = new List<EnumAttributeOption>
               {
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -31,
                     Name = "Not Started"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -32,
                     Name = "In Progress"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -33,
                     Name = "Assigned"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -34,
                     Name = "Complete"
                  },
               }
            }
         };

         enumAttributes.ForEach(attribute =>
         {
            foreach (EnumAttributeOption option in attribute.EnumAttributeOptions)
            {
               option.AttributeId = attribute.AttributeId;
            }
         });

         // EnumCollectionAttributes = MULTI-SELECT of Enum values
         List<EnumCollectionAttribute> enumCollectionAttributes = new List<EnumCollectionAttribute>
         {
            new EnumCollectionAttribute
            {
               AttributeId = -1,
               AttributeDataTypeId = (int)AttributeDataTypes.EnumCollection,
               Name = "Industry",
               ShortName = "IND",
               EnumAttributeOptions = new List<EnumAttributeOption>
               {
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -1,
                     Name = "Advertising"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -2,
                     Name = "Agriculture & Forestry"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -3,
                     Name = "Construction"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -4,
                     Name = "Education - Higher Ed"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -5,
                     Name = "Education - K12"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -6,
                     Name = "Energy, Mining, Oil & Gas"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -7,
                     Name = "Financial Services"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -8,
                     Name = "Government - Federal"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -9,
                     Name = "Government - Local"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -10,
                     Name = "Government - Military"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -11,
                     Name = "Government - State"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -12,
                     Name = "Health Care"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -13,
                     Name = "Insurance"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -14,
                     Name = "Manufacturing - Aerospace"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -15,
                     Name = "Manufacturing - Automotive"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -16,
                     Name = "Manufacturing - Consumer Goods"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -17,
                     Name = "Manufacturing - Industrial"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -18,
                     Name = "Media & Entertainment"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -19,
                     Name = "Membership Organizations"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -20,
                     Name = "Non-Profit"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -21,
                     Name = "Pharmaceuticals & Biotech"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -22,
                     Name = "Professional & Technical Services"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -23,
                     Name = "Real Estate, Rental & Leasing"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -24,
                     Name = "Retail"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -25,
                     Name = "Technology Hardware"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -26,
                     Name = "Technology Software & Services"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -27,
                     Name = "Telecommunications"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -28,
                     Name = "Transportation & Warehousing"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -29,
                     Name = "Travel, Leisure & Hospitality"
                  },
                  new EnumAttributeOption
                  {
                     EnumAttributeOptionId = -30,
                     Name = "Utilities"
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

         // otherDefaultAttributes = arbitrary values, NOT related to Enums
         List<Domain.Attribute> otherDefaultAttributes = new List<Domain.Attribute>
         {
            new StringAttribute
            {
               AttributeId = -2,
               AttributeDataTypeId = (int)AttributeDataTypes.String,
               Name = "Notes",
               ShortName = "NOTES"
            },
            new UtcDateTimeAttribute
            {
               AttributeId = -3,
               AttributeDataTypeId = (int)AttributeDataTypes.UtcDateTime,
               Name = "Due Date",
               ShortName = "DUE"
            }
         };

         AddDefaultAttributes(otherDefaultAttributes);
         AddDefaultAttributes(enumCollectionAttributes);
         AddDefaultAttributes(enumAttributes);
      }
   }
}
