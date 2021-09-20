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

         // RESOURCE

         modelBuilder.Entity<Resource>()
            .HasOne(r => r.ResourceType)
            .WithMany(rt => rt.Resources)
            .HasForeignKey(r => r.ResourceTypeId);
         modelBuilder.Entity<Resource>()
            .Property(r => r.Name).HasMaxLength(ModelConstraints.NAME_LENGTH);
         modelBuilder.Entity<Resource>()
            .Property(r => r.Uri).HasMaxLength(ModelConstraints.URI_LENGTH);
         modelBuilder.Entity<Resource>()
            .HasIndex(r => r.Uri).IsUnique();

         // BUSINESSSERVICE

         modelBuilder.Entity<BusinessService>()
            .Property(e => e.Name).HasMaxLength(ModelConstraints.NAME_LENGTH);

         // BUSINESSSERVICEDEPENDENCY

         modelBuilder.Entity<BusinessServiceDependency>()
            .Property(e => e.DependencyName).HasMaxLength(ModelConstraints.NAME_LENGTH);

         modelBuilder.Entity<BusinessServiceDependency>()
           .HasOne(bsd => bsd.BusinessService)
           .WithMany(bs => bs.BusinessServiceDependencies)
           .HasForeignKey(bsd => bsd.BusinessServiceId);

         modelBuilder.Entity<BusinessServiceDependency>()
            .HasOne(bsd => bsd.ResourceType)
            .WithMany(rt => rt.BusinessServiceDependencies)
            .HasForeignKey(bsd => bsd.ResourceTypeId);
      }

      public virtual DbSet<Environment> Environments
      {
         get;
         set;
      }

      public virtual DbSet<ResourceType> DomainResourceTypes
      {
         get;
         set;
      }
      
      public virtual DbSet<Resource> Resources
      {
         get;
         set;
      }

      public virtual DbSet<BusinessService> BusinessServices
      {
         get;
         set;
      }

      public virtual DbSet<BusinessServiceDependency> BusinessServiceDependencies
      {
         get;
         set;
      }
   }
}
