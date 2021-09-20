using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Domain;
using Sopheon.CloudNative.Environments.Domain.Models;
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
         // ENVIRONMENT 

         modelBuilder.Entity<Environment>()
            .HasIndex(e => e.EnvironmentKey).IsUnique();
         modelBuilder.Entity<Environment>()
            .Property(e => e.Name).HasMaxLength(ModelConstraints.NAME_LENGTH);
         modelBuilder.Entity<Environment>()
            .Property(e => e.Description).HasMaxLength(ModelConstraints.DESCRIPTION_LENGTH);

         // RESOURCETYPE
         
         modelBuilder.Entity<ResourceType>()
            .Property(e => e.Name).HasMaxLength(ModelConstraints.NAME_LENGTH);

         // BUSINESSSERVICE

         modelBuilder.Entity<BusinessService>()
            .Property(e => e.Name).HasMaxLength(ModelConstraints.NAME_LENGTH);

      }

      public virtual DbSet<Environment> Environments
      {
         get;
         set;
      }
   }
}
