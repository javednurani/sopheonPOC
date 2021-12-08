using Microsoft.EntityFrameworkCore;

namespace Sopheon.CloudNative.Products.Domain
{
   public abstract class TenantEnvironmentDbContext : DbContext
   {
      public TenantEnvironmentDbContext(DbContextOptions<ProductManagementContext> options) : base(options)
      {
      }

      protected override void OnConfiguring(DbContextOptionsBuilder modelBuilder)
      {
         if (!modelBuilder.IsConfigured)
         {
            // ...
         }
         base.OnConfiguring(modelBuilder);
      }
   }

   public class ProductManagementContext : TenantEnvironmentDbContext
   {
      public static readonly string DEFAULT_SCHEMA = "SPM"; // Sopheon Product Management

      #region Process Configuration and Domain Value Entities

      public DbSet<ProductItemType> ProductItemType { get; set; }

      public DbSet<Status> Status { get; set; }

      public DbSet<AttributeDataType> AttributeDataType { get; set; }

      public DbSet<Attribute> Attributes { get; set; }

      #endregion

      public DbSet<Product> Products { get; set; }

      public ProductManagementContext(DbContextOptions<ProductManagementContext> options) : base(options)
      {
      }

      protected override void OnConfiguring(DbContextOptionsBuilder modelBuilder)
      {
         modelBuilder.UseSqlServer(options =>
         {
            options.MigrationsHistoryTable(
               schema: ProductManagementContext.DEFAULT_SCHEMA,
               tableName: "DBInstallHistory");
         });

         base.OnConfiguring(modelBuilder);
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);

         modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductManagementContext).Assembly);

         base.OnModelCreating(modelBuilder);
      }
   }
}
