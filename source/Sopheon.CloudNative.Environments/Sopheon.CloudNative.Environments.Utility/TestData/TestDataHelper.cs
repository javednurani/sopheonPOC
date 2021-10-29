using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Data;
using Sopheon.CloudNative.Environments.Domain.Enums;
using Sopheon.CloudNative.Environments.Domain.Models;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Utility.TestData
{
   public static class TestDataHelper
   {
      public static async Task SeedTestDataAsync(string databaseConnection)
      {
         DbContextOptions<EnvironmentContext> _dbContextOptions =
               new DbContextOptionsBuilder<EnvironmentContext>()
                  .UseSqlServer(databaseConnection)
                  .Options;


         using var context = new EnvironmentContext(_dbContextOptions);

         if (!await context.Environments.AnyAsync())
         {
            // ENV.DomainResourceTypes table is populated through Migrations
            DomainResourceType azureSqlResourceType = await context.DomainResourceTypes.FirstAsync(d => d.Id == (int)ResourceTypes.AzureSqlDb);

            // ENV.Resources

            Resource resource1 = new Resource
            {
               Uri = TestDataConstants.RESOURCE_URI_1,
               DomainResourceType = azureSqlResourceType
            };

            Resource resource2 = new Resource
            {
               Uri = TestDataConstants.RESOURCE_URI_2,
               DomainResourceType = azureSqlResourceType
            };

            Resource resource3 = new Resource
            {
               Uri = TestDataConstants.RESOURCE_URI_3,
               DomainResourceType = azureSqlResourceType
            };

            Resource resource4 = new Resource
            {
               Uri = TestDataConstants.RESOURCE_URI_4,
               DomainResourceType = azureSqlResourceType
            };

            // ENV.BusinessServices

            BusinessService businessService1 = new BusinessService
            {
               Name = TestDataConstants.BUSINESS_SERVICE_NAME_1
            };

            BusinessService businessService2 = new BusinessService
            {
               Name = TestDataConstants.BUSINESS_SERVICE_NAME_2
            };

            // ENV.Environments

            Environment environment1 = new Environment
            {
               Name = "Hammer Production",
               Description = "Hammer Corp production environment",
               EnvironmentKey = TestDataConstants.EnvironmentKey1,
               Owner = Guid.NewGuid(),
               IsDeleted = false
            };

            Environment environment2 = new Environment
            {
               Name = "Stark Production",
               Description = "Stark Corp production environment",
               EnvironmentKey = TestDataConstants.EnvironmentKey2,
               Owner = Guid.NewGuid(),
               IsDeleted = false
            };

            Environment environment3 = new Environment
            {
               Name = "Hammer Preview",
               Description = "Hammer Corp preview/UAT environment",
               EnvironmentKey = TestDataConstants.EnvironmentKey3,
               Owner = Guid.NewGuid(),
               IsDeleted = false
            };

            BusinessServiceDependency businessServiceDependency1 = new BusinessServiceDependency
            {
               DependencyName = TestDataConstants.DEPENDENCY_NAME_1,
               BusinessService = businessService1,
               DomainResourceType = azureSqlResourceType
            };

            BusinessServiceDependency businessServiceDependency2 = new BusinessServiceDependency
            {
               DependencyName = TestDataConstants.DEPENDENCY_NAME_2,
               BusinessService = businessService2,
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
               new EnvironmentResourceBinding
               {
                  Environment = environment1,
                  Resource = resource1,
                  BusinessServiceDependency = businessServiceDependency2
               },
               new EnvironmentResourceBinding
               {
                  Environment = environment2,
                  Resource = resource2,
                  BusinessServiceDependency = businessServiceDependency1
               },
               new EnvironmentResourceBinding
               {
                  Environment = environment2,
                  Resource = resource2,
                  BusinessServiceDependency = businessServiceDependency2
               },
            };

            DedicatedEnvironmentResource[] dedicatedEnvironmentResources = new DedicatedEnvironmentResource[]
            {
               new DedicatedEnvironmentResource
               {
                  Environment = environment1,
                  Resource = resource1
               },
               new DedicatedEnvironmentResource
               {
                  Environment = environment2,
                  Resource = resource2
               },
            };

            // Adding EnvironmentResourceBindings will add related Entities (Environments, Resources, BusinessServices, BusinessServiceDependencies
            context.EnvironmentResourceBindings.AddRange(environmentResourceBindings);
            context.DedicatedEnvironmentResources.AddRange(dedicatedEnvironmentResources);

            // Environments and Resources with no bindings are not included in EnvironmentResourceBindings, and need to be added individually
            context.Environments.Add(environment3);
            context.Resources.AddRange(resource3, resource4);
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
