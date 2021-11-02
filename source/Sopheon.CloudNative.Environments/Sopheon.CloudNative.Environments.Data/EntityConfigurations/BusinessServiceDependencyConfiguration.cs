using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Environments.Data.SeedData;
using Sopheon.CloudNative.Environments.Domain;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Data.EntityConfigurations
{
   public class BusinessServiceDependencyConfiguration : BaseConfiguration, IEntityTypeConfiguration<BusinessServiceDependency>
   {
      public void Configure(EntityTypeBuilder<BusinessServiceDependency> builder)
      {
         builder.Property(bsd => bsd.Id)
            .HasColumnName(GetIdColumnName<BusinessServiceDependency>());

         builder.Property(bsd => bsd.DependencyName)
            .HasMaxLength(ModelConstraints.NAME_LENGTH)
            .IsRequired();

         builder.HasOne(bsd => bsd.BusinessService)
           .WithMany(bs => bs.BusinessServiceDependencies)
           .HasForeignKey(bsd => bsd.BusinessServiceId)
           .OnDelete(DeleteBehavior.Restrict);

         builder.HasOne(bsd => bsd.DomainResourceType)
            .WithMany(rt => rt.BusinessServiceDependencies)
            .HasForeignKey(bsd => bsd.DomainResourceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

         builder.HasIndex(bsd => new { bsd.BusinessServiceId, bsd.DependencyName })
            .IsUnique();

         builder.HasData(EnvironmentSeedData.BusinessServiceDependencies);
      }
   }
}
