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
         builder.Property(bsd => bsd.Id).HasColumnName(nameof(BusinessServiceDependency) + "Id");

         builder.Property(bsd => bsd.DependencyName).HasMaxLength(ModelConstraints.NAME_LENGTH);

         builder.HasOne(bsd => bsd.BusinessService)
           .WithMany(bs => bs.BusinessServiceDependencies)
           .HasForeignKey(bsd => bsd.BusinessServiceId)
           .OnDelete(DeleteBehavior.Restrict);

         builder.HasOne(bsd => bsd.ResourceType)
            .WithMany(rt => rt.BusinessServiceDependencies)
            .HasForeignKey(bsd => bsd.ResourceTypeId)
            .OnDelete(DeleteBehavior.Restrict);
      }
   }
}
