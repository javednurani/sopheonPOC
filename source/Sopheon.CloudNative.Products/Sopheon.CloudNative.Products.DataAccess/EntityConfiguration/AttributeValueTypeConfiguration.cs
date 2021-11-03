using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Products.DataAccess.SeedData;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.DataAccess.EntityConfiguration
{
   public class AttributeValueTypeConfiguration : IEntityTypeConfiguration<AttributeValueType>
   {
      public void Configure(EntityTypeBuilder<AttributeValueType> builder)
      {
         builder.HasData(ProductSeedData.SystemAttributeValueTypes);
      }
   }
}
