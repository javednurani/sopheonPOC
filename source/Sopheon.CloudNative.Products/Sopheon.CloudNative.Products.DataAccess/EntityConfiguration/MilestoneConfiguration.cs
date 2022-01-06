using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.DataAccess.EntityConfiguration
{
   public class MilestoneConfiguration :IEntityTypeConfiguration<Milestone>
   {
      public void Configure(EntityTypeBuilder<Milestone> builder)
      {
         builder
            .Property(m => m.Name)
            .HasMaxLength(ModelConstraints.NAME_LENGTH_150)
            .IsRequired();

         builder
            .Property(m => m.Notes)
            .HasMaxLength(ModelConstraints.NOTES_LENGTH_4000);

         builder
            .HasOne(m => m.Product)
            .WithMany(p => p.Milestones)
            .HasForeignKey(m => m.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
      }
   }
}
