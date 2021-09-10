using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Data;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;


namespace Sopheon.CloudNative.Environments.Domain.UnitTests
{
   public class EFEnvironmentRepository_GetEnvironments_UnitTests
   {
      DbContextOptions<EnvironmentContext> _dbContextOptions;

      public EFEnvironmentRepository_GetEnvironments_UnitTests()
      {
         var builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentRepository_GetEnvironments_UnitTests));
         _dbContextOptions = builder.Options;
      }

      [Fact]
      public async Task GetEnvironments_HappyPath_DeletedFilteredAsync()
      {
         using var context = new EnvironmentContext(_dbContextOptions);

         // Arrange - seed test data
         context.AddRange(new[] { randomEnvironment(false), randomEnvironment(false), randomEnvironment(true) });
         context.SaveChanges();

         // Act
         var environments = await new EFEnvironmentRepository(context).GetEnvironments();

         // Assert
         Assert.Equal(2, environments.Count());
      }

      [Fact]
      public async Task GetEnvironments_HappyPath_AllDeletedFilteredAsync()
      {
         using var context = new EnvironmentContext(_dbContextOptions);

         // Arrange - seed test data
         context.AddRange(new[] { randomEnvironment(true), randomEnvironment(true), randomEnvironment(true) });
         context.SaveChanges();

         // Act
         var environments = await new EFEnvironmentRepository(context).GetEnvironments();

         // Assert
         Assert.Empty(environments);
      }

      private static Environment randomEnvironment(bool isDeleted)
      {
         return new Environment
         {
            Name = Some.Random.String(),
            Description = Some.Random.String(),
            EnvironmentKey = Some.Random.Guid(),
            Owner = Some.Random.Guid(),
            IsDeleted = isDeleted
         };
      }
   }
}
