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
   public class EFEnvironmentRepository_UpdateEnvironment_UnitTests
   {
      [Fact]
      public async Task UpdateEnvironment_HappyPath_EnvironmentReturned()
      {
         // Arrange
         var builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentRepository_UpdateEnvironment_UnitTests));
         var options = builder.Options;
         Guid keyToUpdate = SomeRandom.Guid();
         Environment environment = new Environment
         {
            EnvironmentKey = keyToUpdate,
            Name = SomeRandom.String(),
            Description = SomeRandom.String(),
            Owner = SomeRandom.Guid(),
         };
         using (var context = new EnvironmentContext(options))
         {
            var environments = new List<Environment>
                {
                    environment,
                    new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = SomeRandom.Guid(), Owner = SomeRandom.Guid(), IsDeleted = false },
                    new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = SomeRandom.Guid(), Owner = SomeRandom.Guid(), IsDeleted = true },
                };

            context.AddRange(environments);
            context.SaveChanges();
         }

         using (var context = new EnvironmentContext(options))
         {
            var sut = new EFEnvironmentRepository(context);

            // Act
            environment.Name = SomeRandom.String();
            environment.Description = SomeRandom.String();
            environment.Owner = SomeRandom.Guid();

            Environment updateEnvironment = await sut.UpdateEnvironment(environment);

            // Assert
            Assert.Equal(environment.Name, updateEnvironment.Name);
            Assert.Equal(environment.EnvironmentKey, updateEnvironment.EnvironmentKey);
            Assert.Equal(environment.Owner, updateEnvironment.Owner);
            Assert.Equal(environment.Description, updateEnvironment.Description);
            Assert.Equal(environment.EnvironmentID, updateEnvironment.EnvironmentID);
         }
      }

      [Fact]
      public async Task UpdateEnvironment_EnvironmentNotFound_MissingNotFound()
      {
         // Arrange
         var builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentRepository_UpdateEnvironment_UnitTests));
         var options = builder.Options;
         Environment environment = new Environment
         {
            EnvironmentKey = SomeRandom.Guid(),
            Name = SomeRandom.String(),
            Owner = SomeRandom.Guid(),
         };
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
            var sut = new EFEnvironmentRepository(context);

            // Act + Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => sut.UpdateEnvironment(environment));
         }
      }

      [Fact]
      public async Task UpdateEnvironment_EnvironmentNotFound_DeletedNotFound()
      {
         // Arrange
         var builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentRepository_UpdateEnvironment_UnitTests));
         var options = builder.Options;
         Guid deletedKey = SomeRandom.Guid();
         Environment environment = new Environment
         {
            EnvironmentKey = deletedKey,
            Name = SomeRandom.String(),
            Owner = SomeRandom.Guid(),
         };
         using (var context = new EnvironmentContext(options))
         {
            var environments = new List<Environment>
                {
                    new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = deletedKey, Owner = SomeRandom.Guid(), IsDeleted = true },
                    new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = SomeRandom.Guid(), Owner = SomeRandom.Guid(), IsDeleted = false },
                    new Environment { Name = SomeRandom.String(), Description = SomeRandom.String(), EnvironmentKey = SomeRandom.Guid(), Owner = SomeRandom.Guid(), IsDeleted = false },
                };

            context.AddRange(environments);
            context.SaveChanges();
         }

         using (var context = new EnvironmentContext(options))
         {
            var sut = new EFEnvironmentRepository(context);

            // Act + Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => sut.UpdateEnvironment(environment));            
         }
      }
   }
}
