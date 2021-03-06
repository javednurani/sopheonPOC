using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Domain.Models;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Data
{
	public class EnvironmentContext : DbContext
	{
		public static readonly string DEFAULT_SCHEMA = "ENV";

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
				optionsBuilder.UseSqlServer(b =>
					b.MigrationsHistoryTable(
						schema: DEFAULT_SCHEMA,
						tableName: "DBInstallHistory"));
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);

			// apply all configurations defined in current assembly
			modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
		}

		public virtual DbSet<BusinessServiceDependency> BusinessServiceDependencies
		{
			get;
			set;
		}

		public virtual DbSet<BusinessService> BusinessServices
		{
			get;
			set;
		}

		public virtual DbSet<DomainResourceType> DomainResourceTypes
		{
			get;
			set;
		}


		public virtual DbSet<EnvironmentResourceBinding> EnvironmentResourceBindings
		{
			get;
			set;
		}
		public virtual DbSet<Environment> Environments
		{
			get;
			set;
		}

		public virtual DbSet<Resource> Resources
		{
			get;
			set;
		}

		public virtual DbSet<DedicatedEnvironmentResource> DedicatedEnvironmentResources
		{
			get;
			set;
		}
	}
}
