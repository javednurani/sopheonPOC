using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Products.Domain;
using Sopheon.CloudNative.Products.Domain.Attributes.Enum;
using Sopheon.CloudNative.Products.Domain.Attributes.Money;

namespace Sopheon.CloudNative.Products.DataAccess.Extensions
{
   internal static class EntityTypeBuilderExtensions
   {
      public static void OwnsManyAttributeValues<TAttributeContainer>(this EntityTypeBuilder<TAttributeContainer> builder) where TAttributeContainer : class, IAllAttributesContainer
      {
         builder
             .OwnsMany(product => product.Int32AttributeValues)
             .WithOwner();

         builder
             .OwnsMany(product => product.DecimalAttributeValues)
             .WithOwner();

         builder
             .OwnsManyMoneyAttributeValues();

         builder
             .OwnsMany(product => product.StringAttributeValues)
             .WithOwner();

         builder
             .OwnsMany(product => product.UtcDateTimeAttributeValues)
             .WithOwner();

         builder
             .OwnsMany(product => product.EnumCollectionAttributeValues)
             .WithOwner();

         builder
             .OwnsManyEnumCollectionAttributeValues();
      }

      public static void OwnsManyMoneyAttributeValues<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, IMoneyAttributeContainer
      {
         builder
             .OwnsMany(entity => entity.MoneyAttributeValues, moneyAttributeValue =>
             {
                moneyAttributeValue.OwnsOne(mav => mav.Value, value =>
                {
                   value.Property(mv => mv.Value).HasColumnName(nameof(MoneyValue.Value));
                   value.Property(mv => mv.CurrencyCode).HasColumnName(nameof(MoneyValue.CurrencyCode));
                });
             });
      }

      public static void OwnsManyEnumCollectionAttributeValues<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, IEnumCollectionAttributeContainer
      {
         builder
             .OwnsMany(entity => entity.EnumCollectionAttributeValues, enumCollectionAttributeValue =>
             {
                enumCollectionAttributeValue
                       .WithOwner();

                // Set Unique Identifier for '<type> AttributeValue' instance
                string shadowKeyName = $"{typeof(TEntity).Name}{nameof(EnumCollectionAttributeValue)}Id";
                enumCollectionAttributeValue.Property<int>(shadowKeyName);
                enumCollectionAttributeValue.HasKey(shadowKeyName);

                enumCollectionAttributeValue
                       .OwnsMany(v => v.Value, value =>
                     {
                        // Set FK name to avoid redundant default naming of '<fktablename_fkcolumn>'
                        value.Property<int>(shadowKeyName);
                        value
                               .WithOwner()
                               .HasForeignKey(shadowKeyName)
                               .HasPrincipalKey(shadowKeyName);

                        value
                               .HasOne(c => c.EnumAttributeOption)
                               .WithMany()
                               .HasForeignKey(g => g.EnumAttributeOptionId)
                               .OnDelete(DeleteBehavior.NoAction);

                        value.HasKey(shadowKeyName, nameof(EnumAttributeOptionValue.EnumAttributeOptionId));
                     });
             });
      }
   }
}
