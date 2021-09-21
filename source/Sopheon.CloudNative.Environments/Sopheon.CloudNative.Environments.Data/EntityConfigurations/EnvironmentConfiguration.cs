using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Environments.Domain;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Data.EntityConfigurations
{
   public class EnvironmentConfiguration : IEntityTypeConfiguration<Environment>
   {
      public void Configure(EntityTypeBuilder<Environment> builder)
      {
         builder.HasIndex(e => e.EnvironmentKey).IsUnique();

         builder.Property(e => e.Name).HasMaxLength(ModelConstraints.NAME_LENGTH);

         builder.Property(e => e.Description).HasMaxLength(ModelConstraints.DESCRIPTION_LENGTH);
      }
   }
}
