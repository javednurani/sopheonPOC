using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Data.EntityConfigurations
{
   public class EnvironmentResourceBindingConfiguration : BaseConfiguration, IEntityTypeConfiguration<EnvironmentResourceBinding>
   {
      public void Configure(EntityTypeBuilder<EnvironmentResourceBinding> builder)
      {
         builder.Property(erb => erb.Id)
            .HasColumnName(GetIdColumnName<EnvironmentResourceBinding>());

         builder.HasOne(erb => erb.Environment)
            .WithMany(e => e.EnvironmentResourceBindings)
            .HasForeignKey(erb => erb.EnvironmentId)
            .OnDelete(DeleteBehavior.Restrict);

         builder.HasOne(erb => erb.Resource)
            .WithMany(r => r.EnvironmentResourceBindings)
            .HasForeignKey(erb => erb.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);

         builder.HasOne(erb => erb.BusinessServiceDependency)
            .WithMany(bsd => bsd.EnvironmentResourceBindings)
            .HasForeignKey(erb => erb.BusinessServiceDependencyId)
            .OnDelete(DeleteBehavior.Restrict);

         builder.HasIndex(erb => new { erb.EnvironmentId, erb.BusinessServiceDependencyId })
            .IsUnique();
      }
   }
}
