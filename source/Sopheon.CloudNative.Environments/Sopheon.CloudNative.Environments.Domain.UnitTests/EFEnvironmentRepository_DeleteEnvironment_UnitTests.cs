using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Domain.Data;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Domain.UnitTests.TestHelpers;
using System;
using System.Threading.Tasks;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;


namespace Sopheon.CloudNative.Environments.Domain.UnitTests
{
   public class EFEnvironmentRepository_DeleteEnvironment_UnitTests
   {
      DbContextOptions<EnvironmentContext> _dbContextOptions;

      public EFEnvironmentRepository_DeleteEnvironment_UnitTests()
      {
         var builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentRepository_DeleteEnvironment_UnitTests));
         _dbContextOptions = builder.Options;
      }

      [Fact]
      public async Task DeleteEnvironment_HappyPath_IsDeletedTrue()
      {
         using var context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         Guid keyToDelete = SomeRandom.Guid();

         context.AddRange(new[] { new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = keyToDelete, Owner = SomeRandom.Guid(), IsDeleted = false } });
         context.SaveChanges();

         var controller = new EFEnvironmentRepository(context);

         // Act
         await controller.DeleteEnvironment(new Environment { EnvironmentKey = keyToDelete });

         // Assert
         Environment environment = await context.Environments.FirstOrDefaultAsync(env => env.EnvironmentKey == keyToDelete);
         Assert.NotNull(environment);
         Assert.True(environment.IsDeleted);
      }

      [Fact]
      public async Task DeleteEnvironment_EnvironmentNotFound_MissingNotFound()
      {
         using var context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         Environment environment = new Environment
         {
            EnvironmentKey = SomeRandom.Guid(), // find by key will fail
         };

         context.AddRange(new[] { randomEnvironment(false), randomEnvironment(false) });
         context.SaveChanges();

         EFEnvironmentRepository sut = new EFEnvironmentRepository(context);

         // Act + Assert
         await Assert.ThrowsAsync<EntityNotFoundException>(() => sut.DeleteEnvironment(environment));

      }

      [Fact]
      public async Task DeleteEnvironment_EnvironmentNotFound_DeletedNotFound()
      {
         using var context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         Guid deletedKey = SomeRandom.Guid();
         Environment environment = new Environment
         {
            EnvironmentKey = deletedKey, // entity.IsDeleted = true in datastore
         };

         context.AddRange(new[] {
            new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = deletedKey, Owner = SomeRandom.Guid(), IsDeleted = true },
            randomEnvironment(false)
         });
         context.SaveChanges();

         var sut = new EFEnvironmentRepository(context);

         // Act + Assert
         await Assert.ThrowsAsync<EntityNotFoundException>(() => sut.DeleteEnvironment(environment));
      }

      private static Environment randomEnvironment(bool isDeleted)
      {
         return new Environment
         {
            Name = SomeRandom.String(),
            Description = SomeRandom.String(),
            EnvironmentKey = SomeRandom.Guid(),
            Owner = SomeRandom.Guid(),
            IsDeleted = isDeleted
         };
      }
   }
}
