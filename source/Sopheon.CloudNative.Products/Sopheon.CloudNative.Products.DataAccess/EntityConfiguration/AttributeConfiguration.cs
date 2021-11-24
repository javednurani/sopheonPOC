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
         builder.Property(a => a.Name)
            .HasMaxLength(ModelConstraints.NAME_LENGTH_60)
            .IsRequired();

         builder.HasData(ProductSeedData.DefaultAttributes);
      }
   }
}
