using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Products.DataAccess.SeedData;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.DataAccess.EntityConfiguration
{
   public class ProductItemTypeConfiguration : IEntityTypeConfiguration<ProductItemType>
   {
      public void Configure(EntityTypeBuilder<ProductItemType> builder)
      {
         builder.HasData(ProductSeedData.SystemManagedProductItemTypes);
      }
   }
}
