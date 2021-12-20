using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Products.DataAccess.SeedData;
using Sopheon.CloudNative.Products.Domain;
using Sopheon.CloudNative.Products.Domain.Attributes.Decimal;
using Sopheon.CloudNative.Products.Domain.Attributes.Enum;
using Sopheon.CloudNative.Products.Domain.Attributes.Int32;
using Sopheon.CloudNative.Products.Domain.Attributes.Money;
using Sopheon.CloudNative.Products.Domain.Attributes.String;
using Sopheon.CloudNative.Products.Domain.Attributes.UtcDateTime;

namespace Sopheon.CloudNative.Products.DataAccess.EntityConfiguration
{
   public class AttributeConfiguration : IEntityTypeConfiguration<Attribute>
   {
      public void Configure(EntityTypeBuilder<Attribute> builder)
      {
         builder
            .ToTable(nameof(Attribute))
            .HasDiscriminator(attribute => attribute.AttributeDataTypeId)
            .HasValue<StringAttribute>((int)AttributeDataTypes.String)
            .HasValue<Int32Attribute>((int)AttributeDataTypes.Int32)
            .HasValue<DecimalAttribute>((int)AttributeDataTypes.Decimal)
            .HasValue<MoneyAttribute>((int)AttributeDataTypes.Money)
            .HasValue<UtcDateTimeAttribute>((int)AttributeDataTypes.UtcDateTime)
            .HasValue<EnumCollectionAttribute>((int)AttributeDataTypes.EnumCollection);

         builder.Property(a => a.Name)
            .HasMaxLength(ModelConstraints.NAME_LENGTH_60)
            .IsRequired();
      }
   }

   public class EnumCollectionAttributeConfiguration : IEntityTypeConfiguration<EnumCollectionAttribute>
   {
      public void Configure(EntityTypeBuilder<EnumCollectionAttribute> builder)
      {
         builder
             .HasMany(enumCollectionAttribute => enumCollectionAttribute.EnumAttributeOptions)
             .WithOne(option => option.EnumCollectionAttribute)
             .HasForeignKey(option => option.AttributeId)
             .HasPrincipalKey(enumCollectionAttribute => enumCollectionAttribute.AttributeId);

         EnumCollectionAttribute[] attributesWithoutOptions = ProductSeedData.GetDefaultAttributes<EnumCollectionAttribute>()
             .Select(a => a.ShallowCopy())
             .Cast<EnumCollectionAttribute>()
             .ToArray();
         foreach (var attribute in attributesWithoutOptions)
         {
            attribute.EnumAttributeOptions = null;
         }
         //System.Diagnostics.Debugger.Launch();
         builder.HasData(attributesWithoutOptions);
      }
   }

   public class EnumAttributeOptionConfiguration : IEntityTypeConfiguration<EnumAttributeOption>
   {
      public void Configure(EntityTypeBuilder<EnumAttributeOption> builder)
      {
         builder
             .HasIndex(nameof(EnumAttributeOption.AttributeId), nameof(EnumAttributeOption.Name))
             .IsUnique();

         //System.Diagnostics.Debugger.Launch();
         var data = ProductSeedData.GetDefaultAttributes<EnumCollectionAttribute>().SelectMany(a => a.EnumAttributeOptions).ToArray();
         builder.HasData(data);
      }
   }

   public class StringAttributeConfiguration : IEntityTypeConfiguration<StringAttribute>
   {
      public void Configure(EntityTypeBuilder<StringAttribute> builder)
      {
         builder.HasData(ProductSeedData.GetDefaultAttributes<StringAttribute>());
      }
   }

   public class MoneyAttributeConfiguration : IEntityTypeConfiguration<MoneyAttribute>
   {
      public void Configure(EntityTypeBuilder<MoneyAttribute> builder)
      {
         builder.HasData(ProductSeedData.GetDefaultAttributes<MoneyAttribute>());
      }
   }

   public class UtcDateTimeAttributeConfiguration : IEntityTypeConfiguration<UtcDateTimeAttribute>
   {
      public void Configure(EntityTypeBuilder<UtcDateTimeAttribute> builder)
      {
         builder.HasData(ProductSeedData.GetDefaultAttributes<UtcDateTimeAttribute>());
      }
   }

   public class Int32AttributeConfiguration : IEntityTypeConfiguration<Int32Attribute>
   {
      public void Configure(EntityTypeBuilder<Int32Attribute> builder)
      {
         builder.HasData(ProductSeedData.GetDefaultAttributes<Int32Attribute>());
      }
   }
}
