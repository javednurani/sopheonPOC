using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Environments.Domain;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Data.EntityConfigurations
{
   public class BusinessServiceConfiguration : IEntityTypeConfiguration<BusinessService>
   {
      public void Configure(EntityTypeBuilder<BusinessService> builder)
      {
         builder.Property(e => e.Name).HasMaxLength(ModelConstraints.NAME_LENGTH);
      }
   }
}
