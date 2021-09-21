using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Environments.Domain;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Data.EntityConfigurations
{
   public class BusinessServiceDependencyConfiguration : IEntityTypeConfiguration<BusinessServiceDependency>
   {
      public void Configure(EntityTypeBuilder<BusinessServiceDependency> builder)
      {
         builder.Property(e => e.DependencyName).HasMaxLength(ModelConstraints.NAME_LENGTH);

         builder.HasOne(bsd => bsd.BusinessService)
           .WithMany(bs => bs.BusinessServiceDependencies)
           .HasForeignKey(bsd => bsd.BusinessServiceId);

         builder.HasOne(bsd => bsd.ResourceType)
            .WithMany(rt => rt.BusinessServiceDependencies)
            .HasForeignKey(bsd => bsd.ResourceTypeId);
      }
   }
}
