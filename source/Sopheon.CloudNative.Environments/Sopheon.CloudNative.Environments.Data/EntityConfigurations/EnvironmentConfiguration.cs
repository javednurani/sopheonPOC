using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Environments.Domain;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Data.EntityConfigurations
{
   public class EnvironmentConfiguration : BaseConfiguration, IEntityTypeConfiguration<Environment>
   {
      public void Configure(EntityTypeBuilder<Environment> builder)
      {
         builder.Property(e => e.Id).HasColumnName(GetIdColumnName<Environment>());

         builder.HasIndex(e => e.EnvironmentKey).IsUnique();

         builder.Property(e => e.Name).HasMaxLength(ModelConstraints.NAME_LENGTH);

         builder.Property(e => e.Description).HasMaxLength(ModelConstraints.DESCRIPTION_LENGTH);
      }
   }
}
