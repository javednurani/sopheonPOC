using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Environments.Domain;
using Sopheon.CloudNative.Environments.Domain.Enums;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Data.EntityConfigurations
{
   public class BusinessServiceConfiguration : BaseConfiguration, IEntityTypeConfiguration<BusinessService>
   {
      public void Configure(EntityTypeBuilder<BusinessService> builder)
      {
         builder.Property(bs => bs.Id)
            .HasColumnName(GetIdColumnName<BusinessService>());

         builder.HasIndex(bs => bs.Name)
            .IsUnique();

         builder.Property(bs => bs.Name)
            .HasMaxLength(ModelConstraints.NAME_LENGTH)
            .IsRequired();

         // Seed domain data to BusinessServices table generated from BusinessServices enum
         BusinessServices[] businessServices = (BusinessServices[])Enum.GetValues(typeof(BusinessServices));
         builder.HasData(
            businessServices.Select(bs => new BusinessService
            {
               Id = (int)bs,
               Name = bs.ToString()
            })
         );
         // TODO CLOUD-2037
         // builder.HasData(EnvironmentSeedData.BusinessServices);
      }
   }
}
