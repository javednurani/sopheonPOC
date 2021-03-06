using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Data;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;


namespace Sopheon.CloudNative.Environments.Data.UnitTests
{
   public class EFEnvironmentRepository_DeleteEnvironment_UnitTests
   {
      DbContextOptions<EnvironmentContext> _dbContextOptions;

      public EFEnvironmentRepository_DeleteEnvironment_UnitTests()
      {
         DbContextOptionsBuilder<EnvironmentContext> builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentRepository_DeleteEnvironment_UnitTests));
         _dbContextOptions = builder.Options;
      }

      [Fact]
      public async Task DeleteEnvironment_HappyPath_IsDeletedTrue()
      {
         using EnvironmentContext context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         Guid keyToDelete = Some.Random.Guid();

         context.AddRange(new[] { new Environment { Name = Some.Random.String(), Description = Some.Random.String(), EnvironmentKey = keyToDelete, Owner = Some.Random.Guid(), IsDeleted = false } });
         context.SaveChanges();

         // Act
         await new EFEnvironmentRepository(context).DeleteEnvironment(keyToDelete);

         // Assert
         Environment environment = await context.Environments.FirstOrDefaultAsync(env => env.EnvironmentKey == keyToDelete);
         Assert.NotNull(environment);
         Assert.True(environment.IsDeleted);
      }

      [Fact]
      public async Task DeleteEnvironment_HappyPath_SoftDeleteIsPersisted()
      {
         using EnvironmentContext context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         Guid keyToDelete = Some.Random.Guid();

         context.AddRange(new[] { new Environment { Name = Some.Random.String(), Description = Some.Random.String(), EnvironmentKey = keyToDelete, Owner = Some.Random.Guid(), IsDeleted = false } });
         context.SaveChanges();

         // Act
         await new EFEnvironmentRepository(context).DeleteEnvironment(keyToDelete);

         // reset environment context, which will only contain persisted data from original context
         using EnvironmentContext context2 = new EnvironmentContext(_dbContextOptions);
         Environment deletedEnvironment = await context2.Environments.SingleOrDefaultAsync(env => env.EnvironmentKey == keyToDelete);

         // Assert
         Assert.NotNull(deletedEnvironment);
         Assert.True(deletedEnvironment.IsDeleted);
      }

      [Fact]
      public async Task DeleteEnvironment_EnvironmentNotFound_ByKey_EntityNotFoundException()
      {
         using EnvironmentContext context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         // find by key will fail
         context.AddRange(new[] { Some.Random.Environment(false), Some.Random.Environment(false) });
         context.SaveChanges();

         EFEnvironmentRepository sut = new EFEnvironmentRepository(context);

         // Act + Assert
         await Assert.ThrowsAsync<EntityNotFoundException>(() => sut.DeleteEnvironment(Some.Random.Guid()));

      }

      [Fact]
      public async Task DeleteEnvironment_EnvironmentNotFound_IsDeleted_EntityNotFoundException()
      {
         using EnvironmentContext context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         Guid deletedKey = Some.Random.Guid();

         // entity.IsDeleted = true in datastore
         context.AddRange(new[] {
            new Environment { Name = Some.Random.String(), Description = Some.Random.String(), EnvironmentKey = deletedKey, Owner = Some.Random.Guid(), IsDeleted = true },
            Some.Random.Environment(false)
         });
         context.SaveChanges();

         EFEnvironmentRepository sut = new EFEnvironmentRepository(context);

         // Act + Assert
         await Assert.ThrowsAsync<EntityNotFoundException>(() => sut.DeleteEnvironment(deletedKey));
      }
   }
}
