using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Data;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Data.UnitTests
{
   public class EFEnvironmentRepository_UpdateEnvironment_UnitTests
   {
      DbContextOptions<EnvironmentContext> _dbContextOptions;

      public EFEnvironmentRepository_UpdateEnvironment_UnitTests()
      {
         var builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentRepository_UpdateEnvironment_UnitTests));
         _dbContextOptions = builder.Options;
      }

      [Fact]
      public async Task UpdateEnvironment_HappyPath_EnvironmentReturned()
      {
         // Arrange
         Environment environment = Some.Random.Environment();

         using var context = new EnvironmentContext(_dbContextOptions);
         context.AddRange(new[] { environment, Some.Random.Environment(false), Some.Random.Environment(true) });
         context.SaveChanges();

         // Act - change environment values
         environment.Name = Some.Random.String();
         environment.Description = Some.Random.String();
         environment.Owner = Some.Random.Guid();

         var sut = new EFEnvironmentRepository(context);
         Environment updateEnvironment = await sut.UpdateEnvironment(environment);

         // Assert
         Assert.Equal(environment.Name, updateEnvironment.Name);
         Assert.Equal(environment.EnvironmentKey, updateEnvironment.EnvironmentKey);
         Assert.Equal(environment.Owner, updateEnvironment.Owner);
         Assert.Equal(environment.Description, updateEnvironment.Description);
         Assert.Equal(environment.EnvironmentID, updateEnvironment.EnvironmentID);
      }

      [Fact]
      public async Task UpdateEnvironment_HappyPath_EnvironmentUpdateIsPersisted()
      {
         // Arrange
         Environment environment = Some.Random.Environment();
         using var context = new EnvironmentContext(_dbContextOptions);
         context.AddRange(new[] { environment, Some.Random.Environment(false), Some.Random.Environment(true) });
         context.SaveChanges();

         // Act - change environment values
         environment.Name = Some.Random.String();
         environment.Description = Some.Random.String();
         environment.Owner = Some.Random.Guid();

         var sut = new EFEnvironmentRepository(context);
         _ = await sut.UpdateEnvironment(environment);

         // Assert

         // reset environment context, which will only contain persisted data from original context
         using var context2 = new EnvironmentContext(_dbContextOptions);
         Environment retrievedEnvironment = context2.Environments.Single(e => e.EnvironmentKey == environment.EnvironmentKey);

         // ensure the updated values were retireved
         Assert.Equal(environment.Name, retrievedEnvironment.Name);
         Assert.Equal(environment.EnvironmentKey, retrievedEnvironment.EnvironmentKey);
         Assert.Equal(environment.Owner, retrievedEnvironment.Owner);
         Assert.Equal(environment.Description, retrievedEnvironment.Description);
         Assert.Equal(environment.EnvironmentID, retrievedEnvironment.EnvironmentID);
      }

      [Fact]
      public async Task UpdateEnvironment_EnvironmentNotFound_MissingNotFound()
      {
         // Arrange
         using var context = new EnvironmentContext(_dbContextOptions);
         context.AddRange(new[] { Some.Random.Environment(false), Some.Random.Environment(false), Some.Random.Environment(true) });
         context.SaveChanges();

         // Act + Assert
         Environment nonexistentEnvironment = Some.Random.Environment();
         var sut = new EFEnvironmentRepository(context);
         await Assert.ThrowsAsync<EntityNotFoundException>(() => sut.UpdateEnvironment(nonexistentEnvironment));
      }

      [Fact]
      public async Task UpdateEnvironment_EnvironmentNotFound_DeletedNotFound()
      {
         // Arrange
         Environment deletedEnvironment = Some.Random.Environment(true);
         using var context = new EnvironmentContext(_dbContextOptions);
         context.AddRange(new[] { deletedEnvironment, Some.Random.Environment(false), Some.Random.Environment(false) });
         context.SaveChanges();

         // Act + Assert
         var sut = new EFEnvironmentRepository(context);
         await Assert.ThrowsAsync<EntityNotFoundException>(() => sut.UpdateEnvironment(deletedEnvironment));
      }
   }
}
