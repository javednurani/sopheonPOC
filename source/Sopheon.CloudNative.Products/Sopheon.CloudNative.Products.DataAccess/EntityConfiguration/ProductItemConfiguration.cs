using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.DataAccess.EntityConfiguration
{
   public class ProductItemConfiguration : IEntityTypeConfiguration<ProductItem>
   {
      public void Configure(EntityTypeBuilder<ProductItem> builder)
      {
         ConfigureOwnedAttributeProperties<ProductItem>(builder);
      }

      private static void ConfigureOwnedAttributeProperties<TAttributeContainer>(EntityTypeBuilder<TAttributeContainer> builder) where TAttributeContainer : class, IAttributeContainer
      {
         builder
            .OwnsMany(product => product.IntAttributeValues);

         builder
             .OwnsMany(product => product.DecimalAttributeValues);

         builder
             .OwnsMany(product => product.MoneyAttributeValues, moneyAttributeValue =>
             {
                moneyAttributeValue.OwnsOne(mav => mav.Value, value =>
                {
                   value.Property(mv => mv.Value).HasColumnName("Value");
                   value.Property(mv => mv.CurrencyCode).HasColumnName("CurrencyCode");
                });
             });

         builder
             .OwnsMany(product => product.StringAttributeValues);

         builder
             .OwnsMany(product => product.UtcDateTimeAttributeValues);
      }
   }
}
