using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Products.DataAccess.SeedData;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.DataAccess.EntityConfiguration
{
   public class AttributeDataTypeConfiguration : IEntityTypeConfiguration<AttributeDataType>
   {
      public void Configure(EntityTypeBuilder<AttributeDataType> builder)
      {
         builder.HasData(ProductSeedData.SystemAttributeDataTypes);
      }
   }
}
