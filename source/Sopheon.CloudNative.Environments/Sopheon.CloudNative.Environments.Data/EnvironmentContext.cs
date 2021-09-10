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
        public virtual DbSet<Environment> Environments
        {
            get;
            set;
        }
    }
}
