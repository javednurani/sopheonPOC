using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Products.DataAccess.Extensions;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.DataAccess.EntityConfiguration
{
   public class ProductConfiguration : IEntityTypeConfiguration<Product>
   {
      public void Configure(EntityTypeBuilder<Product> builder)
      {
         builder.OwnsManyAttributeValues();

         builder.OwnsMany(product => product.KeyPerformanceIndicators);

         builder.HasIndex(p => p.Key)
            .IsUnique();

         builder.Property(p => p.Name)
            .HasMaxLength(ModelConstraints.NAME_LENGTH_300)
            .IsRequired();
      }
   }
}
