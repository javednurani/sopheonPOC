using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Data.UnitTests
{
   public class EFEnvironmentRepository_AddEnvironment_UnitTests
   {
      DbContextOptions<EnvironmentContext> _dbContextOptions;

      public EFEnvironmentRepository_AddEnvironment_UnitTests()
      {
         var builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentRepository_AddEnvironment_UnitTests));
         _dbContextOptions = builder.Options;
      }

      [Fact]
      public async Task AddEnvironment_HappyPath_EnvironmentReturnedWithKey()
      {
         using var context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         Environment environment = new Environment
         {
            Name = Some.Random.String(),
            Owner = Some.Random.Guid(),
            Description = Some.Random.String()
         };

         // Act
         EFEnvironmentRepository sut = new EFEnvironmentRepository(context);
         Environment addEnvironment = await sut.AddEnvironment(environment);

         // Assert
         Assert.Equal(environment.Name, addEnvironment.Name);
         Assert.NotEqual(System.Guid.Empty, addEnvironment.EnvironmentKey);
         Assert.Equal(environment.Owner, addEnvironment.Owner);
         Assert.Equal(environment.Description, addEnvironment.Description);
      }

      [Fact]
      public async Task AddEnvironment_HappyPath_EnvironmentUpdateIsPersisted()
      {
         using var context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         Environment environment = Some.Random.Environment();

         // Act
         EFEnvironmentRepository sut = new EFEnvironmentRepository(context);
         await sut.AddEnvironment(environment);

         // reset environment context, which will only contain persisted data from original context
         using var context2 = new EnvironmentContext(_dbContextOptions);
         Environment retrievedEnvironment = context2.Environments.Single(e => e.EnvironmentKey == environment.EnvironmentKey);

         // Assert
         Assert.Equal(environment.Name, retrievedEnvironment.Name);
         Assert.Equal(environment.EnvironmentKey, retrievedEnvironment.EnvironmentKey);
         Assert.Equal(environment.Owner, retrievedEnvironment.Owner);
         Assert.Equal(environment.Description, retrievedEnvironment.Description);
         Assert.Equal(environment.Id, retrievedEnvironment.Id);
      }
   }
}
