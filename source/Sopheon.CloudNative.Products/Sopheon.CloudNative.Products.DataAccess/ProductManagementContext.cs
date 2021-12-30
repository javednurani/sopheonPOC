using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
      public DbSet<Task> Tasks { get; set; }

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

         modelBuilder.ApplyKindUtcToAllDateTimes();

         base.OnModelCreating(modelBuilder);
      }
   }

   public static class ModelBuilderExtensions
   {
      /// <summary>
      /// Applies the DateTimeKind.Utc to all DateTime and DateTime? model properties
      /// </summary>
      /// <param name="modelBuilder"></param>
      public static void ApplyKindUtcToAllDateTimes(this ModelBuilder modelBuilder)
      {
         var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

         var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

         foreach (var entityType in modelBuilder.Model.GetEntityTypes())
         {
            if (entityType.IsKeyless) { continue; }

            foreach (var property in entityType.GetProperties())
            {
               if (property.ClrType == typeof(DateTime))
               {
                  property.SetValueConverter(dateTimeConverter);
               }
               else if (property.ClrType == typeof(DateTime?))
               {
                  property.SetValueConverter(nullableDateTimeConverter);
               }
            }
         }
      }
   }
}
