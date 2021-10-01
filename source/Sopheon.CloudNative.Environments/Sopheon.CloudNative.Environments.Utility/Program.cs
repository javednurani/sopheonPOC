using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Data;
using Sopheon.CloudNative.Environments.Domain.Enums;
using Sopheon.CloudNative.Environments.Domain.Models;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

[assembly:ExcludeFromCodeCoverage]
namespace Sopheon.CloudNative.Environments.Utility
{

   class Program
   {
	   private static string _databaseConnection = "";
      static async System.Threading.Tasks.Task Main(string[] args)
      {
	      if (args.Any(arg => arg == "-Database"))
	      {
		      _databaseConnection = System.Environment.GetEnvironmentVariable("LocalDatabaseConnectionString");
	      }

      DbContextOptions<EnvironmentContext> _dbContextOptions =
            new DbContextOptionsBuilder<EnvironmentContext>()
               .UseSqlServer(_databaseConnection)
               .Options;

         using var context = new EnvironmentContext(_dbContextOptions);

         if (!await context.Environments.AnyAsync())
         {
            DomainResourceType azureSqlResourceType = await context.DomainResourceTypes.FirstAsync(d => d.Id == (int)ResourceTypes.AzureSqlDb);

            Resource resource1 = new Resource
            {
               Uri = TestData.RESOURCE_URI_1,
               DomainResourceType = resourceType1
            };

            BusinessService businessService1 = new BusinessService
            {
               Name = TestData.BUSINESS_SERVICE_NAME_1
            };

            Environment environment1 = new Environment
            {
               Name = "Hammer Production",
               Description = "Hammer Corp production environment",
               EnvironmentKey = TestData.EnvironmentKey1,
               Owner = Guid.NewGuid(),
               IsDeleted = false
            };

            BusinessServiceDependency businessServiceDependency1 = new BusinessServiceDependency
            {
               DependencyName = TestData.DEPENDENCY_NAME_1,
               BusinessService = businessService1,
               DomainResourceType = azureSqlResourceType
            };

            EnvironmentResourceBinding[] environmentResourceBindings = new EnvironmentResourceBinding[]
            {
               new EnvironmentResourceBinding
               {
                  Environment = environment1,
                  Resource = resource1,
                  BusinessServiceDependency = businessServiceDependency1
               },
            };

            context.EnvironmentResourceBindings.AddRange(environmentResourceBindings);
            int result = await context.SaveChangesAsync();
            Console.WriteLine(result + " entries written to the database.");
         }
         else
         {
            Console.WriteLine("0 entries written. Database is not empty.");
         }
      }
   }
}
