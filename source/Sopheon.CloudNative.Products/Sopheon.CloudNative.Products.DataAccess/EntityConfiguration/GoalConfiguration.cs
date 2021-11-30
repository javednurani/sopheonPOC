using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Products.DataAccess.SeedData;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.DataAccess.EntityConfiguration
{
   public class GoalConfiguration : IEntityTypeConfiguration<Goal>
   {
      public void Configure(EntityTypeBuilder<Goal> builder)
      {
         builder.Property(a => a.Name)
            .HasMaxLength(ModelConstraints.NAME_LENGTH_300)
            .IsRequired();
      }
   }
}
