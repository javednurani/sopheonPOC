using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Data;
using Sopheon.CloudNative.Environments.Domain.Enums;
using Sopheon.CloudNative.Environments.Domain.Models;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Utility
{
   [ExcludeFromCodeCoverage]
   class Program
   {
      static async System.Threading.Tasks.Task Main(string[] args)
      {
         DbContextOptions<EnvironmentContext> _dbContextOptions =
            new DbContextOptionsBuilder<EnvironmentContext>()
               .UseSqlServer("YOUR_CONN_STRING_HERE")
               .Options;

         using var context = new EnvironmentContext(_dbContextOptions);

         if (!await context.Environments.AnyAsync())
         {
            // We might want to have all seeded resource types available but not yet.
            //ResourceTypes[] resourceTypes = (ResourceTypes[])Enum.GetValues(typeof(ResourceTypes));
            //IEnumerable<DomainResourceType> domainResourceTypes = resourceTypes.Select(r => new DomainResourceType
            //{
            //   Id = (int)r,
            //   Name = r.ToString()
            //});

            DomainResourceType azureSqlResourceType = await context.DomainResourceTypes.FirstAsync(d => d.Id == (int)ResourceTypes.AzureSqlDb);

            Resource resource1 = new Resource
            {
               Uri = TestData.RESOURCE_URI_1,
               DomainResourceType = azureSqlResourceType
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
            await context.SaveChangesAsync();
         }
      }
   }
}
