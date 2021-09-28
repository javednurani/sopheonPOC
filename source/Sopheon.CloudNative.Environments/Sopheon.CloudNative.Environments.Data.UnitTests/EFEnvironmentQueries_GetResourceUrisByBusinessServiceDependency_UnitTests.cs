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
         DbContextOptionsBuilder<EnvironmentContext> builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentQueries_GetResourceUrisByBusinessServiceDependency_UnitTests));
         _dbContextOptions = builder.Options;
      }

      [Fact]
      public async Task GetResourceUrisByBusinessServiceDependency_HappyPath_ResourceUrisReturned()
      {
         using EnvironmentContext context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         string businessServiceName = Some.Random.String();

         string dependencyName1 = Some.Random.String();
         string dependencyName2 = Some.Random.String();

         string resourceUri1 = Some.Random.String();
         string resourceUri2 = Some.Random.String();
         string resourceUri3 = Some.Random.String();

         DomainResourceType domainResourceType = Some.Random.DomainResourceType();

         BusinessService businessService = new BusinessService
         {
            Name = businessServiceName
         };

         Environment environment1 = Some.Random.Environment();

         Environment environment2 = Some.Random.Environment();

         IEnumerable<EnvironmentResourceBinding> environmentResourceBindings = new List<EnvironmentResourceBinding>
         {
            // 2 EnvironmentResourceBindings for BusinessServiceDependency having Name = dependencyName1
            new EnvironmentResourceBinding
            {
               Environment = environment1,
               Resource = new Resource
               {
                  Uri = resourceUri1,
                  DomainResourceType = domainResourceType,
               },
               BusinessServiceDependency = new BusinessServiceDependency
               {
                  DependencyName = dependencyName1,
                  BusinessService = businessService,
                  DomainResourceType = domainResourceType,
               },
            },
            new EnvironmentResourceBinding
            {
               Environment = environment2,
               Resource = new Resource
               {
                  Uri = resourceUri2,
                  DomainResourceType = domainResourceType,
               },
               BusinessServiceDependency = new BusinessServiceDependency
               {
                  DependencyName = dependencyName1,
                  BusinessService = businessService,
                  DomainResourceType = domainResourceType,
               },
            },
            // 1 EnvironmentResourceBinding for BusinessServiceDependency having Name = dependencyName2
            new EnvironmentResourceBinding
            {
               Environment = environment1,
               Resource = new Resource
               {
                  Uri = resourceUri3,
                  DomainResourceType = domainResourceType,
               },
               BusinessServiceDependency = new BusinessServiceDependency
               {
                  DependencyName = dependencyName2,
                  BusinessService = businessService,
                  DomainResourceType = domainResourceType,
               },
            }
         };

         context.AddRange(environmentResourceBindings);
         await context.SaveChangesAsync();

         // Act
         EFEnvironmentQueries sut = new EFEnvironmentQueries(context);
         IEnumerable<string> resourceUris = await sut.GetResourceUrisByBusinessServiceDependency(businessServiceName, dependencyName1);

         // Assert
         Assert.Equal(2, resourceUris.Count());
         Assert.Contains(resourceUri1, resourceUris);
         Assert.Contains(resourceUri2, resourceUris);
      }

      [Fact]
      public async Task GetResourceUrisByBusinessServiceDependency_BusinessServiceNotFound_NothingReturned()
      {
         using EnvironmentContext context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         string businessServiceName = Some.Random.String();

         string dependencyName = Some.Random.String();

         DomainResourceType domainResourceType = Some.Random.DomainResourceType();

         BusinessService businessService = new BusinessService
         {
            Name = Some.Random.String()
         };

         Environment environment = Some.Random.Environment();

         EnvironmentResourceBinding environmentResourceBinding = new EnvironmentResourceBinding
         {
            Environment = environment,
            Resource = new Resource
            {
               Uri = Some.Random.String(),
               DomainResourceType = domainResourceType,
            },
            BusinessServiceDependency = new BusinessServiceDependency
            {
               DependencyName = dependencyName,
               BusinessService = businessService,
               DomainResourceType = domainResourceType,
            },
         };

         context.Add(environmentResourceBinding);
         await context.SaveChangesAsync();

         // Act
         EFEnvironmentQueries sut = new EFEnvironmentQueries(context);
         IEnumerable<string> resourceUris = await sut.GetResourceUrisByBusinessServiceDependency(businessServiceName, dependencyName);

         // Assert
         Assert.Empty(resourceUris);
      }

      [Fact]
      public async Task GetResourceUrisByBusinessServiceDependency_BusinessServiceDependencyNotFound_NothingReturned()
      {
         using EnvironmentContext context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         string businessServiceName = Some.Random.String();

         string dependencyName = Some.Random.String();

         DomainResourceType domainResourceType = Some.Random.DomainResourceType();

         BusinessService businessService = new BusinessService
         {
            Name = businessServiceName
         };

         Environment environment = Some.Random.Environment();

         EnvironmentResourceBinding environmentResourceBinding = new EnvironmentResourceBinding
         {
            Environment = environment,
            Resource = new Resource
            {
               Uri = Some.Random.String(),
               DomainResourceType = domainResourceType,
            },
            BusinessServiceDependency = new BusinessServiceDependency
            {
               DependencyName = Some.Random.String(),
               BusinessService = businessService,
               DomainResourceType = domainResourceType,
            },
         };

         context.Add(environmentResourceBinding);
         await context.SaveChangesAsync();

         // Act
         EFEnvironmentQueries sut = new EFEnvironmentQueries(context);
         IEnumerable<string> resourceUris = await sut.GetResourceUrisByBusinessServiceDependency(businessServiceName, dependencyName);

         // Assert
         Assert.Empty(resourceUris);
      }
   }
}
