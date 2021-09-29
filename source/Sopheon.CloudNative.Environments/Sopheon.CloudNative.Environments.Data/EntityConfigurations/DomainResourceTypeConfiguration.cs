using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Environments.Domain;
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
      }
   }
}
