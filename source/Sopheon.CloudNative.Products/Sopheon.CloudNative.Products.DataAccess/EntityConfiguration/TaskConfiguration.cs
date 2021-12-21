using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.DataAccess.EntityConfiguration
{
   public class TaskConfiguration :IEntityTypeConfiguration<Task>
   {
      public void Configure(EntityTypeBuilder<Task> builder)
      {
         builder
            .Property(t => t.Name)
            .HasMaxLength(ModelConstraints.NAME_LENGTH_150)
            .IsRequired();

         builder
            .Property(t => t.Notes)
            .HasMaxLength(ModelConstraints.NOTES_LENGTH_5000);

         builder
            .HasOne(t => t.Product)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

         builder.ToTable(t => t.IsTemporal());
      }
   }
}
