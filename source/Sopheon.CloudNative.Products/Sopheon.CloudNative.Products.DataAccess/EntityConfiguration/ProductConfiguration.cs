using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.DataAccess.EntityConfiguration
{
   public class ProductConfiguration : IEntityTypeConfiguration<Product>
   {
      public void Configure(EntityTypeBuilder<Product> builder)
      {
         ConfigureOwnedAttributeProperties<Product>(builder);

         builder.HasIndex(p => p.Key)
            .IsUnique();

         builder.Property(p => p.Name)
            .HasMaxLength(ModelConstraints.NAME_LENGTH_300)
            
            .IsRequired();
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
