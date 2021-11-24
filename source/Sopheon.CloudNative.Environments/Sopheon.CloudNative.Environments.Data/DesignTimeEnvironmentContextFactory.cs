using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sopheon.CloudNative.Environments.Data
{
   public class DesignTimeEnvironmentContextFactory : IDesignTimeDbContextFactory<EnvironmentContext>
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="args">May be passed in by "dotnet ef arg1 arg2 -- --extraArg1 extraArg1Value" commands; See https://docs.microsoft.com/en-us/ef/core/cli/dotnet#aspnet-core-environment</param>
      /// <returns></returns>
      public EnvironmentContext CreateDbContext(string[] args)
      {
         var optionsBuilder = new DbContextOptionsBuilder<EnvironmentContext>();

         // Example Usages:
         // dotnet ef database update --force -- --connectionstring "Server=.;Database=TenantEnvironment1;Integrated Security=True"
         // dotnet ef database update -- --connectionstring "Server=.;Database=TenantEnvironment1;Integrated Security=True"
         // dotnet ef database update -- --connectionstring "Server=.;Database=env;User Id=sa;Password=###; Trusted_Connection=False;MultipleActiveResultSets=true"
         string connectionString = args.Length >= 2 && args[0].Equals("--connectionstring", System.StringComparison.OrdinalIgnoreCase) ? args[1] : string.Empty;
         optionsBuilder.UseSqlServer(connectionString,
			b => b.MigrationsHistoryTable(
				"DBInstallHistory",
				EnvironmentContext.DEFAULT_SCHEMA));

			return new EnvironmentContext(optionsBuilder.Options);
      }
   }
}
