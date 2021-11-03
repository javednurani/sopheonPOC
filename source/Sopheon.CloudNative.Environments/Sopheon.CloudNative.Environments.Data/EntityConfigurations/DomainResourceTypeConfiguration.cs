using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Environments.Domain;
using Sopheon.CloudNative.Environments.Domain.Enums;
using Sopheon.CloudNative.Environments.Domain.Infrastructure;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Data.EntityConfigurations
{
   public class DomainResourceTypeConfiguration : BaseConfiguration, IEntityTypeConfiguration<DomainResourceType>
   {
      public void Configure(EntityTypeBuilder<DomainResourceType> builder)
      {
         builder.Property(rt => rt.Id)
            .HasColumnName(GetIdColumnName<DomainResourceType>());

         builder.Property(rt => rt.Name)
            .HasMaxLength(ModelConstraints.NAME_LENGTH)
            .IsRequired();

         // Seed domain data to ENV.DomainResourceTypes table generated from ResourceTypes enum
         ResourceTypes[] resourceTypes = (ResourceTypes[])Enum.GetValues(typeof(ResourceTypes));            
         builder.HasData(
            resourceTypes.Select(r => new DomainResourceType { 
               Id = (int)r, 
               Name = r.ToString(),
               IsDedicated = r.GetAttribute<DedicatedAttribute>().IsDedicated
            })
         );
      }
   }
}
