using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Products.DataAccess.SeedData;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.DataAccess.EntityConfiguration
{
   public class AttributeConfiguration : IEntityTypeConfiguration<Attribute>
   {
      public void Configure(EntityTypeBuilder<Attribute> builder)
      {
         builder.HasData(ProductSeedData.DefaultAttributes);
      }
   }
}
