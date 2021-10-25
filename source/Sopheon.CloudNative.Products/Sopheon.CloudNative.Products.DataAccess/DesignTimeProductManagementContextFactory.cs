using Sopheon.CloudNative.Products.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics;

namespace Sopheon.CloudNative.Products.DataAccess
{
   public class DesignTimeProductManagementContextFactory : IDesignTimeDbContextFactory<ProductManagementContext>
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="args">May be passed in by "dotnet ef arg1 arg2 -- --extraArg1 extraArg1Value" commands; See https://docs.microsoft.com/en-us/ef/core/cli/dotnet#aspnet-core-environment</param>
      /// <returns></returns>
      public ProductManagementContext CreateDbContext(string[] args)
      {
         //Debugger.Launch();
         var optionsBuilder = new DbContextOptionsBuilder<ProductManagementContext>();

         // Example Usages:
         // dotnet ef database update --force -- --connectionstring "Server=.;Database=TenantEnvironment1;Integrated Security=True"
         // dotnet ef database update -- --connectionstring "Server=.;Database=TenantEnvironment1;Integrated Security=True"
         string connectionString = args.Length >= 2 && args[0].Equals("--connectionstring", System.StringComparison.OrdinalIgnoreCase) ? args[1] : string.Empty;
         optionsBuilder.UseSqlServer(connectionString);

         return new ProductManagementContext(optionsBuilder.Options);
      }
   }
}
