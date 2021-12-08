using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Products.DataAccess.Extensions;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.DataAccess.EntityConfiguration
{
   public class ProductItemConfiguration : IEntityTypeConfiguration<ProductItem>
   {
      public void Configure(EntityTypeBuilder<ProductItem> builder)
      {
         builder.OwnsManyAttributeValues();
      }
   }
}
