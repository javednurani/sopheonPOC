using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;


namespace Sopheon.CloudNative.Environments.Domain.Data
{
    class EnvironmentContext : DbContext
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
        public DbSet<Environment> Environments
        {
            get;
            set;
        }
    }
}
