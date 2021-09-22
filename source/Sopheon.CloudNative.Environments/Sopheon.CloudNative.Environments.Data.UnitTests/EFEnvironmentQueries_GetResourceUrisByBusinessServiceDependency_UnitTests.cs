using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Domain.Models;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Data.UnitTests
{
   public class EFEnvironmentQueries_GetResourceUrisByBusinessServiceDependency_UnitTests
   {
      DbContextOptions<EnvironmentContext> _dbContextOptions;


      public EFEnvironmentQueries_GetResourceUrisByBusinessServiceDependency_UnitTests()
      {
         var builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentQueries_GetResourceUrisByBusinessServiceDependency_UnitTests));
         _dbContextOptions = builder.Options;
      }

      [Fact]
      public async Task GetResourceUrisByBusinessServiceDependency_HappyPath_ResourceUrisReturned()
      {
         using var context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         string businessServiceName = Some.Random.String();

         string dependencyName1 = Some.Random.String();
         string dependencyName2 = Some.Random.String();

         string resourceUri1 = Some.Random.String();
         string resourceUri2 = Some.Random.String();
         string resourceUri3 = Some.Random.String();

         ResourceType resourceType = new ResourceType
         {
            Name = Some.Random.String(),
         };

         BusinessService businessService = new BusinessService
         {
            Name = businessServiceName
         };

         Environment environment1 = new Environment
         {
            Name = Some.Random.String(),
            EnvironmentKey = Some.Random.Guid(),
            IsDeleted = false,
            Owner = Some.Random.Guid()
         };

         Environment environment2 = new Environment
         {
            Name = Some.Random.String(),
            EnvironmentKey = Some.Random.Guid(),
            IsDeleted = false,
            Owner = Some.Random.Guid()
         };

         IEnumerable<EnvironmentResourceBinding> environmentResourceBindings = new List<EnvironmentResourceBinding>
         {
            // 2 EnvironmentResourceBindings for BusinessServiceDependency having Name = dependencyName1
            new EnvironmentResourceBinding
            {
               Environment = environment1,
               Resource = new Resource
               {
                  Name = Some.Random.String(),
                  Uri = resourceUri1,
                  ResourceType = resourceType,
               },
               BusinessServiceDependency = new BusinessServiceDependency
               {
                  DependencyName = dependencyName1,
                  BusinessService = businessService,
                  ResourceType = resourceType,
               },
            },
            new EnvironmentResourceBinding
            {
               Environment = environment2,
               Resource = new Resource
               {
                  Name = Some.Random.String(),
                  Uri = resourceUri2,
                  ResourceType = resourceType,
               },
               BusinessServiceDependency = new BusinessServiceDependency
               {
                  DependencyName = dependencyName1,
                  BusinessService = businessService,
                  ResourceType = resourceType,
               },
            },
            // 1 EnvironmentResourceBinding for BusinessServiceDependency having Name = dependencyName2
            new EnvironmentResourceBinding
            {
               Environment = environment1,
               Resource = new Resource
               {
                  Name = Some.Random.String(),
                  Uri = resourceUri3,
                  ResourceType = resourceType,
               },
               BusinessServiceDependency = new BusinessServiceDependency
               {
                  DependencyName = dependencyName2,
                  BusinessService = businessService,
                  ResourceType = resourceType,
               },
            }
         };

         context.AddRange(environmentResourceBindings);
         await context.SaveChangesAsync();

         // Act
         var sut = new EFEnvironmentQueries(context);
         var resourceUris = await sut.GetResourceUrisByBusinessServiceDependency(businessServiceName, dependencyName1);

         // Assert
         Assert.Equal(2, resourceUris.Count());
         Assert.Contains(resourceUri1, resourceUris);
         Assert.Contains(resourceUri2, resourceUris);
      }

      [Fact]
      public async Task GetResourceUrisByBusinessServiceDependency_BusinessServiceNotFound_NothingReturned()
      {
         using var context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         string businessServiceName = Some.Random.String();

         string dependencyName = Some.Random.String();

         ResourceType resourceType = new ResourceType
         {
            Name = Some.Random.String(),
         };

         BusinessService businessService = new BusinessService
         {
            Name = Some.Random.String()
         };

         Environment environment = new Environment
         {
            Name = Some.Random.String(),
            EnvironmentKey = Some.Random.Guid(),
            IsDeleted = false,
            Owner = Some.Random.Guid()
         };

         EnvironmentResourceBinding environmentResourceBinding = new EnvironmentResourceBinding
         {
            Environment = environment,
            Resource = new Resource
            {
               Name = Some.Random.String(),
               Uri = Some.Random.String(),
               ResourceType = resourceType,
            },
            BusinessServiceDependency = new BusinessServiceDependency
            {
               DependencyName = dependencyName,
               BusinessService = businessService,
               ResourceType = resourceType,
            },
         };

         context.Add(environmentResourceBinding);
         await context.SaveChangesAsync();

         // Act
         var sut = new EFEnvironmentQueries(context);
         var resourceUris = await sut.GetResourceUrisByBusinessServiceDependency(businessServiceName, dependencyName);

         // Assert
         Assert.Empty(resourceUris);
      }

      [Fact]
      public async Task GetResourceUrisByBusinessServiceDependency_BusinessServiceDependencyNotFound_NothingReturned()
      {
         using var context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         string businessServiceName = Some.Random.String();

         string dependencyName = Some.Random.String();

         ResourceType resourceType = new ResourceType
         {
            Name = Some.Random.String(),
         };

         BusinessService businessService = new BusinessService
         {
            Name = businessServiceName
         };

         Environment environment = new Environment
         {
            Name = Some.Random.String(),
            EnvironmentKey = Some.Random.Guid(),
            IsDeleted = false,
            Owner = Some.Random.Guid()
         };

         EnvironmentResourceBinding environmentResourceBinding = new EnvironmentResourceBinding
         {
            Environment = environment,
            Resource = new Resource
            {
               Name = Some.Random.String(),
               Uri = Some.Random.String(),
               ResourceType = resourceType,
            },
            BusinessServiceDependency = new BusinessServiceDependency
            {
               DependencyName = Some.Random.String(),
               BusinessService = businessService,
               ResourceType = resourceType,
            },
         };

         context.Add(environmentResourceBinding);
         await context.SaveChangesAsync();

         // Act
         var sut = new EFEnvironmentQueries(context);
         var resourceUris = await sut.GetResourceUrisByBusinessServiceDependency(businessServiceName, dependencyName);

         // Assert
         Assert.Empty(resourceUris);
      }

      [Fact]
      public async Task GetResourceUrisByBusinessServiceDependency_BothParametersEmptyStrings_NothingReturned()
      {
         using var context = new EnvironmentContext(_dbContextOptions);

         // Arrange

         ResourceType resourceType = new ResourceType
         {
            Name = Some.Random.String(),
         };

         BusinessService businessService = new BusinessService
         {
            Name = Some.Random.String()
         };

         Environment environment = new Environment
         {
            Name = Some.Random.String(),
            EnvironmentKey = Some.Random.Guid(),
            IsDeleted = false,
            Owner = Some.Random.Guid()
         };

         EnvironmentResourceBinding environmentResourceBinding = new EnvironmentResourceBinding
         {
            Environment = environment,
            Resource = new Resource
            {
               Name = Some.Random.String(),
               Uri = Some.Random.String(),
               ResourceType = resourceType,
            },
            BusinessServiceDependency = new BusinessServiceDependency
            {
               DependencyName = Some.Random.String(),
               BusinessService = businessService,
               ResourceType = resourceType,
            },
         };

         context.Add(environmentResourceBinding);
         await context.SaveChangesAsync();

         // Act
         var sut = new EFEnvironmentQueries(context);
         var resourceUris = await sut.GetResourceUrisByBusinessServiceDependency(string.Empty, string.Empty);

         // Assert
         Assert.Empty(resourceUris);
      }
   }
}
