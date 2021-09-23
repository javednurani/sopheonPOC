using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Data;
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
            ResourceType resourceType1 = new ResourceType
            {
               Name = "AZURE_SQL_DATABASE"
            };
            ResourceType resourceType2 = new ResourceType
            {
               Name = "AZURE_BLOB_STORAGE"
            };

            Resource resource1 = new Resource
            {
               Uri = "https://hammer-prod-sql.database.windows.net",
               ResourceType = resourceType1
            };

            Resource resource2 = new Resource
            {
               Uri = "https://stark-prod-sql.database.windows.net",
               ResourceType = resourceType1
            };

            Resource resource3 = new Resource
            {
               Uri = "https://hammer-prod-storage.web.core.windows.net",
               ResourceType = resourceType2
            };

            Resource resource4 = new Resource
            {
               Uri = "https://stark-prod-storage.web.core.windows.net",
               ResourceType = resourceType2
            };

            BusinessService businessService1 = new BusinessService
            {
               Name = "PRODUCT_SERVICE"
            };
            BusinessService businessService2 = new BusinessService
            {
               Name = "COMMENT_SERVICE"
            };

            Environment environment1 = new Environment
            {
               Name = "Hammer Production",
               Description = "Hammer Corp production environment",
               EnvironmentKey = Guid.NewGuid(),
               Owner = Guid.NewGuid(),
               IsDeleted = false
            };
            Environment environment2 = new Environment
            {
               Name = "Stark Production",
               Description = "Stark Corp production environment",
               EnvironmentKey = Guid.NewGuid(),
               Owner = Guid.NewGuid(),
               IsDeleted = false
            };


            BusinessServiceDependency businessServiceDependency1 = new BusinessServiceDependency
            {
               DependencyName = "PRODUCT_DATASTORE",
               BusinessService = businessService1,
               ResourceType = resourceType1
            };
            BusinessServiceDependency businessServiceDependency2 = new BusinessServiceDependency
            {
               DependencyName = "COMMENT_DATASTORE",
               BusinessService = businessService2,
               ResourceType = resourceType1
            };
            BusinessServiceDependency businessServiceDependency3 = new BusinessServiceDependency
            {
               DependencyName = "PRODUCT_MEDIASTORE",
               BusinessService = businessService1,
               ResourceType = resourceType2
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
               Environment = environment1,
               Resource = resource3,
               BusinessServiceDependency = businessServiceDependency3
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
            new EnvironmentResourceBinding
            {
               Environment = environment2,
               Resource = resource4,
               BusinessServiceDependency = businessServiceDependency3
            }
            };

            context.EnvironmentResourceBindings.AddRange(environmentResourceBindings);
            await context.SaveChangesAsync();
         }
      }
   }
}
