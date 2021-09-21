using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Environments.Domain;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Data.EntityConfigurations
{
   public class ResourceTypeConfiguration : IEntityTypeConfiguration<ResourceType>
   {
      public void Configure(EntityTypeBuilder<ResourceType> builder)
      {
         builder.Property(e => e.Name).HasMaxLength(ModelConstraints.NAME_LENGTH);
      }
   }
}
