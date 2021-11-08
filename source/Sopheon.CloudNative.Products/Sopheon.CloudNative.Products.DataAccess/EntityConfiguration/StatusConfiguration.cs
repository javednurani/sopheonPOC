using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Products.DataAccess.SeedData;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.DataAccess.EntityConfiguration
{
   public class StatusConfiguration : IEntityTypeConfiguration<Status>
   {
      public void Configure(EntityTypeBuilder<Status> builder)
      {
         // Seed Defaults
         builder.HasData(ProductSeedData.SystemManagedStatuses);
      }
   }
}
