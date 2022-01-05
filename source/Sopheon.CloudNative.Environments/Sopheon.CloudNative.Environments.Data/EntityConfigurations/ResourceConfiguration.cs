using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Environments.Domain;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Data.EntityConfigurations
{
   public class ResourceConfiguration : BaseConfiguration, IEntityTypeConfiguration<Resource>
   {
      public void Configure(EntityTypeBuilder<Resource> builder)
      {
         builder.Property(r => r.Id)
            .HasColumnName(GetIdColumnName<Resource>());

         builder.Property(r => r.Uri)
            .HasMaxLength(ModelConstraints.URI_LENGTH)
            .IsRequired();

         builder.Property(r => r.IsAssigned)
            .IsRequired();

         builder.Property(r => r.Name)
            .IsRequired();

         builder.HasIndex(r => r.Uri)
            .IsUnique();

         builder.HasOne(r => r.DomainResourceType)
            .WithMany(rt => rt.Resources)
            .HasForeignKey(r => r.DomainResourceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

         builder.HasOne(r => r.DedicatedEnvironmentResource)
            .WithOne(der => der.Resource)
            .HasForeignKey<DedicatedEnvironmentResource>(der => der.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);
      }
   }
}
