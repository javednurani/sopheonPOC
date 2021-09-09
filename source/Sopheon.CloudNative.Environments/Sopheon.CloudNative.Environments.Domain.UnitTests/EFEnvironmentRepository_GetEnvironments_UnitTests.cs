using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Domain.Data;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Domain.UnitTests.TestHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;


namespace Sopheon.CloudNative.Environments.Domain.UnitTests
{
   public class EFEnvironmentRepository_GetEnvironments_UnitTests
   {

      [Fact]
      public async Task GetEnvironments_HappyPath_DeletedFilteredAsync()
      {
         // Arrange
         var builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentRepository_GetEnvironments_UnitTests));
         var options = builder.Options;

         using (var context = new EnvironmentContext(options))
         {
            var environments = new List<Environment>
                {
                    new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = SomeRandom.Guid(), Owner = SomeRandom.Guid(), IsDeleted = false },
                    new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = SomeRandom.Guid(), Owner = SomeRandom.Guid(), IsDeleted = false },
                    new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = SomeRandom.Guid(), Owner = SomeRandom.Guid(), IsDeleted = true },
                };

            context.AddRange(environments);
            context.SaveChanges();
         }

         using (var context = new EnvironmentContext(options))
         {
            var controller = new EFEnvironmentRepository(context);
            
            // Act
            var environments = await controller.GetEnvironments();

            // Assert
            Assert.Equal(2, environments.Count());
         }
      }

      [Fact]
      public async Task GetEnvironments_HappyPath_AllDeletedFilteredAsync()
      {
         // Arrange
         var builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentRepository_GetEnvironments_UnitTests));
         var options = builder.Options;

         using (var context = new EnvironmentContext(options))
         {
            var environments = new List<Environment>
                {
                    new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = SomeRandom.Guid(), Owner = SomeRandom.Guid(), IsDeleted = true },
                    new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = SomeRandom.Guid(), Owner = SomeRandom.Guid(), IsDeleted = true },
                    new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = SomeRandom.Guid(), Owner = SomeRandom.Guid(), IsDeleted = true },
                };

            context.AddRange(environments);
            context.SaveChanges();
         }

         using (var context = new EnvironmentContext(options))
         {
            var controller = new EFEnvironmentRepository(context);

            // Act
            var environments = await controller.GetEnvironments();

            // Assert
            Assert.Empty(environments);
         }
      }

   }
}
