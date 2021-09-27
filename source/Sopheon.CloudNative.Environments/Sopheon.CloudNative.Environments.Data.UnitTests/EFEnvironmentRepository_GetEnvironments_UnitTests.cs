using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Domain.Models;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;


namespace Sopheon.CloudNative.Environments.Data.UnitTests
{
   public class EFEnvironmentRepository_GetEnvironments_UnitTests
   {
      DbContextOptions<EnvironmentContext> _dbContextOptions;

      public EFEnvironmentRepository_GetEnvironments_UnitTests()
      {
         DbContextOptionsBuilder<EnvironmentContext> builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentRepository_GetEnvironments_UnitTests));
         _dbContextOptions = builder.Options;
      }

      [Fact]
      public async Task GetEnvironments_HappyPath_DeletedFilteredAsync()
      {
         using EnvironmentContext context = new EnvironmentContext(_dbContextOptions);

         // Arrange - seed test data
         context.AddRange(new[] { Some.Random.Environment(false), Some.Random.Environment(false), Some.Random.Environment(true) });
         context.SaveChanges();

         // Act
         IEnumerable<Environment> environments = await new EFEnvironmentRepository(context).GetEnvironments();

         // Assert
         Assert.Equal(2, environments.Count());
      }

      [Fact]
      public async Task GetEnvironments_HappyPath_AllDeletedFilteredAsync()
      {
         using EnvironmentContext context = new EnvironmentContext(_dbContextOptions);

         // Arrange - seed test data
         context.AddRange(new[] { Some.Random.Environment(true), Some.Random.Environment(true), Some.Random.Environment(true) });
         context.SaveChanges();

         // Act
         IEnumerable<Environment> environments = await new EFEnvironmentRepository(context).GetEnvironments();

         // Assert
         Assert.Empty(environments);
      }
   }
}
