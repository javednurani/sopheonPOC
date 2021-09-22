using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

         Environment environment = new Environment
         {
            Name = Some.Random.String(),
            EnvironmentKey = Some.Random.Guid(),
            IsDeleted = false,
            Owner = Some.Random.Guid()
         };

         ResourceType resourceType = new ResourceType
         {

         };

         Resource resource = new Resource
         {

         };

         BusinessService businessService = new BusinessService
         {

         };

         BusinessServiceDependency businessServiceDependency = new BusinessServiceDependency
         {

         };

         EnvironmentResourceBinding environmentResourceBinding = new EnvironmentResourceBinding
         {

         };


         // Act

         var sut = new EFEnvironmentQueries(context);
         var resourceUris = await sut.GetResourceUrisByBusinessServiceDependency("", "");

         // Assert

         Assert.Single(resourceUris);
      }
   }
}
