using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Products.Domain.Attributes.Decimal;
using Sopheon.CloudNative.Products.Domain.Attributes.Enum;
using Sopheon.CloudNative.Products.Domain.Attributes.Int32;
using Sopheon.CloudNative.Products.Domain.Attributes.Money;
using Sopheon.CloudNative.Products.Domain.Attributes.String;
using Sopheon.CloudNative.Products.Domain.Attributes.UtcDateTime;

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

      // TODO - is table for base entity needed? or derived entity config creates?
      //public DbSet<Attribute> Attributes { get; set; }
      public DbSet<Int32Attribute> Int32Attributes { get; set; }
      public DbSet<StringAttribute> StringAttributes { get; set; }
      public DbSet<DecimalAttribute> DecimalAttributes { get; set; }
      public DbSet<UtcDateTimeAttribute> UtcDateTimeAttributes { get; set; }
      public DbSet<MoneyAttribute> MoneyAttributes { get; set; }
      public DbSet<EnumCollectionAttribute> EnumCollectionAttributes { get; set; }

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
