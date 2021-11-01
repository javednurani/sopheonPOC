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
      // TODO, as Test Data is finalized, split SeedTestDataAsync into smaller Helpers per Entity
      public static async Task SeedTestDataAsync(string databaseConnection)
      {
         DbContextOptions<EnvironmentContext> _dbContextOptions =
               new DbContextOptionsBuilder<EnvironmentContext>()
                  .UseSqlServer(databaseConnection)
                  .Options;


         using var context = new EnvironmentContext(_dbContextOptions);

         if (!await context.Environments.AnyAsync())
         {
            // Fetch Domain data from tables populated through Migrations

            // ENV.DomainResourceTypes 
            DomainResourceType azureSqlResourceType = await context.DomainResourceTypes.FirstAsync(d => d.Id == (int)ResourceTypes.AzureSqlDb);
            DomainResourceType azureBlobResourceType = await context.DomainResourceTypes.FirstAsync(d => d.Id == (int)ResourceTypes.AzureBlobStorage);

            // ENV.BusinessServices
            BusinessService productManagementService = await context.BusinessServices.FirstAsync(bs => bs.Id == (int)BusinessServices.ProductManagement);

            // Populate Test data with TestDataConstants

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

            Resource resource5 = new Resource
            {
               Uri = TestDataConstants.RESOURCE_URI_5,
               DomainResourceType = azureBlobResourceType
            };

            Resource resource6 = new Resource
            {
               Uri = TestDataConstants.RESOURCE_URI_6,
               DomainResourceType = azureBlobResourceType
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
               Name = "Acme Production",
               Description = "Acme Corp production environment",
               EnvironmentKey = TestDataConstants.EnvironmentKey3,
               Owner = Guid.NewGuid(),
               IsDeleted = false
            };

            // Relational Entities

            // ENV.BusinesServiceDependencies
            BusinessServiceDependency businessServiceDependency1 = new BusinessServiceDependency
            {
               DependencyName = TestDataConstants.DEPENDENCY_NAME_1,
               BusinessService = productManagementService,
               DomainResourceType = azureSqlResourceType
            };

            // ENV.EnvironmentResourceBindings
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
                  Environment = environment2,
                  Resource = resource2,
                  BusinessServiceDependency = businessServiceDependency1
               }
            };

            // ENV.DedicatedEnvironmentResources
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

            // Adding EnvironmentResourceBindings will add related Entities (Environments, Resources, BusinessServices, BusinessServiceDependencies)
            context.EnvironmentResourceBindings.AddRange(environmentResourceBindings);
            context.DedicatedEnvironmentResources.AddRange(dedicatedEnvironmentResources);

            // Environments and Resources not included in any EnvironmentResourceBindings need to be added individually
            context.Environments.Add(environment3);
            context.Resources.AddRange(resource3, resource4, resource5, resource6);

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
