using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Data;
using Sopheon.CloudNative.Environments.Domain.Models;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Utility
{
   [ExcludeFromCodeCoverage]
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
            DomainResourceType resourceType1 = new DomainResourceType
            {
               Name = "AZURE_SQL_DATABASE"
            };
            DomainResourceType resourceType2 = new DomainResourceType
            {
               Name = "AZURE_BLOB_STORAGE"
            };

            Resource resource1 = new Resource
            {
               Uri = TestData.RESOURCE_URI_1,
               DomainResourceType = resourceType1
            };

            Resource resource2 = new Resource
            {
               Uri = "https://stark-prod-sql.database.windows.net",
               DomainResourceType = resourceType1
            };

            Resource resource3 = new Resource
            {
               Uri = "https://hammer-prod-storage.web.core.windows.net",
               DomainResourceType = resourceType2
            };

            Resource resource4 = new Resource
            {
               Uri = "https://stark-prod-storage.web.core.windows.net",
               DomainResourceType = resourceType2
            };

            BusinessService businessService1 = new BusinessService
            {
               Name = TestData.BUSINESS_SERVICE_NAME_1
            };
            BusinessService businessService2 = new BusinessService
            {
               Name = "COMMENT_SERVICE"
            };

            Environment environment1 = new Environment
            {
               Name = "Hammer Production",
               Description = "Hammer Corp production environment",
               EnvironmentKey = Guid.Parse("11111111-1111-1111-1111-111111111111"), // TODO: consolidate
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
               DependencyName = TestData.DEPENDENCY_NAME_1,
               BusinessService = businessService1,
               DomainResourceType = resourceType1
            };
            BusinessServiceDependency businessServiceDependency2 = new BusinessServiceDependency
            {
               DependencyName = "COMMENT_DATASTORE",
               BusinessService = businessService2,
               DomainResourceType = resourceType1
            };
            BusinessServiceDependency businessServiceDependency3 = new BusinessServiceDependency
            {
               DependencyName = "PRODUCT_MEDIASTORE",
               BusinessService = businessService1,
               DomainResourceType = resourceType2
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
