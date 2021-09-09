using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Domain.Data;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Domain.UnitTests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;


namespace Sopheon.CloudNative.Environments.Domain.UnitTests
{
   public class EFEnvironmentRepository_DeleteEnvironment_UnitTests
   {
      [Fact]
      public async Task DeleteEnvironment_HappyPath_IsDeletedTrue()
      {
         // Arrange
         var builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentRepository_DeleteEnvironment_UnitTests));
         var options = builder.Options;

         Guid keyToDelete = SomeRandom.Guid();

         using (var context = new EnvironmentContext(options))
         {
            var environments = new List<Environment>
            {
               new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = keyToDelete, Owner = SomeRandom.Guid(), IsDeleted = false }
            };

            context.AddRange(environments);
            context.SaveChanges();
         }

         using (var context = new EnvironmentContext(options))
         {
            var controller = new EFEnvironmentRepository(context);

            // Act
            await controller.DeleteEnvironment(new Environment { EnvironmentKey = keyToDelete });

            // Assert
            Environment environment = await context.Environments.FirstOrDefaultAsync(env => env.EnvironmentKey == keyToDelete);
            Assert.NotNull(environment);
            Assert.True(environment.IsDeleted);
         }
      }

      [Fact]
      public async Task DeleteEnvironment_EnvironmentNotFound_MissingNotFound()
      {
         // Arrange
         var builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentRepository_DeleteEnvironment_UnitTests));
         var options = builder.Options;

         Environment environment = new Environment
         {
            EnvironmentKey = SomeRandom.Guid(), // find by key will fail
         };

         using (var context = new EnvironmentContext(options))
         {
            var environments = new List<Environment>
            {
               new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = SomeRandom.Guid(), Owner = SomeRandom.Guid(), IsDeleted = false },
               new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = SomeRandom.Guid(), Owner = SomeRandom.Guid(), IsDeleted = false }
            };

            context.AddRange(environments);
            context.SaveChanges();
         }

         using (var context = new EnvironmentContext(options))
         {
            var sut = new EFEnvironmentRepository(context);

            // Act + Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => sut.DeleteEnvironment(environment));
         }
      }

      [Fact]
      public async Task DeleteEnvironment_EnvironmentNotFound_DeletedNotFound()
      {
         // Arrange
         var builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentRepository_DeleteEnvironment_UnitTests));
         var options = builder.Options;

         Guid deletedKey = SomeRandom.Guid();
         Environment environment = new Environment
         {
            EnvironmentKey = deletedKey, // entity.IsDeleted = true in datastore
         };
         using (var context = new EnvironmentContext(options))
         {
            var environments = new List<Environment>
            {
               new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = deletedKey, Owner = SomeRandom.Guid(), IsDeleted = true },
               new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = SomeRandom.Guid(), Owner = SomeRandom.Guid(), IsDeleted = false }
            };

            context.AddRange(environments);
            context.SaveChanges();
         }

         using (var context = new EnvironmentContext(options))
         {
            var sut = new EFEnvironmentRepository(context);

            // Act + Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => sut.DeleteEnvironment(environment));
         }
      }
   }
}
