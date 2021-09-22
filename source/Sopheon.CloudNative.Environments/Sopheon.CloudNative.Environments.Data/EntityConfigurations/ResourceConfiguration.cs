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
         builder.Property(r => r.Id).HasColumnName(GetIdColumnName<Resource>());

         builder.Property(r => r.Name).HasMaxLength(ModelConstraints.NAME_LENGTH);

         builder.Property(r => r.Uri).HasMaxLength(ModelConstraints.URI_LENGTH);

         builder.HasIndex(r => r.Uri).IsUnique();

         builder.HasOne(r => r.ResourceType)
            .WithMany(rt => rt.Resources)
            .HasForeignKey(r => r.ResourceTypeId)
            .OnDelete(DeleteBehavior.Restrict);
      }
   }
}
