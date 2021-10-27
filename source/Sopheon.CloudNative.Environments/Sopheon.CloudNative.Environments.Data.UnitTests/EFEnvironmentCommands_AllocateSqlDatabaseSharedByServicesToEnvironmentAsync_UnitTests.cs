using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Data.UnitTests
{
   public class EFEnvironmentCommands_AllocateSqlDatabaseSharedByServicesToEnvironmentAsync_UnitTests
   {
      DbContextOptions<EnvironmentContext> _dbContextOptions;

      public EFEnvironmentCommands_AllocateSqlDatabaseSharedByServicesToEnvironmentAsync_UnitTests()
      {
         DbContextOptionsBuilder<EnvironmentContext> builder = new DbContextOptionsBuilder<EnvironmentContext>();
         builder.UseInMemoryDatabase(nameof(EFEnvironmentCommands_AllocateSqlDatabaseSharedByServicesToEnvironmentAsync_UnitTests));
         _dbContextOptions = builder.Options;
      }

      [Fact]
      public async Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync_HappyPath_EnvironmentReturned()
      {
         using EnvironmentContext context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         Environment environment = Some.Random.Environment();

         context.Add(environment);
         await context.SaveChangesAsync();

         EFEnvironmentCommands sut = new EFEnvironmentCommands(context);

         // Act + Assert
         // No Exception implies success
         await sut.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(environment.EnvironmentKey);
      }

      [Fact]
      public async Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync_EnvironmentNotFound_ThrowsException()
      {
         using EnvironmentContext context = new EnvironmentContext(_dbContextOptions);

         // Arrange
         Environment environment = Some.Random.Environment();

         context.Add(environment);
         await context.SaveChangesAsync();

         EFEnvironmentCommands sut = new EFEnvironmentCommands(context);

         // Act + Assert
         await Assert.ThrowsAsync<EntityNotFoundException>(() => sut.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(Some.Random.Guid()));
      }
   }
}
