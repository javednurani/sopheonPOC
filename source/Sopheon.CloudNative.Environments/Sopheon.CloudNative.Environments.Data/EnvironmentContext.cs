using Microsoft.EntityFrameworkCore;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;


namespace Sopheon.CloudNative.Environments.Data
{
   public class EnvironmentContext : DbContext
   {
      public EnvironmentContext()
      {
      }

      public EnvironmentContext(DbContextOptions<EnvironmentContext> options) : base(options)
      {
      }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         if (!optionsBuilder.IsConfigured)
         {
            optionsBuilder.UseSqlServer();
         }
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Environment>()
            .HasIndex(e => e.EnvironmentKey).IsUnique();
         modelBuilder.Entity<Environment>()
            .Property(e => e.Name).HasMaxLength(64);  // TODO Pull max lengths from constants file in domain project for validation
         modelBuilder.Entity<Environment>()
            .Property(e => e.Description).HasMaxLength(1000);
      }

      public virtual DbSet<Environment> Environments
      {
         get;
         set;
      }
   }
}
