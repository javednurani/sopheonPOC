using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Data.EntityConfigurations
{
   public class DedicatedEnvironmentResourceConfiguration : BaseConfiguration, IEntityTypeConfiguration<DedicatedEnvironmentResource>
   {
      public void Configure(EntityTypeBuilder<DedicatedEnvironmentResource> builder)
      {
         builder.Property(d => d.Id)
            .HasColumnName(GetIdColumnName<DedicatedEnvironmentResource>());

         builder.HasOne(der => der.Environment)
            .WithMany(e => e.DedicatedEnvironmentResources)
            .HasForeignKey(der => der.EnvironmentId)
            .OnDelete(DeleteBehavior.Restrict);

         builder.HasIndex(d => d.ResourceId)
            .IsUnique();

         builder.Property(d => d.ResourceId)
            .IsRequired();

         builder.Property(d => d.EnvironmentId)
            .IsRequired();
      }
   }
}
