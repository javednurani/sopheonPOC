using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Commands;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Helpers
{
   public class AllocateSqlDatabaseSharedByServicesToEnvironmentHelper_AllocateSqlDatabaseSharedByServicesToEnvironmentAsync_UnitTests
   {
      private ResourceAllocationHelper _sut;
      private Mock<ILogger<ResourceAllocationHelper>> _logger;
      private Mock<IEnvironmentCommands> _mockEnvironmentCommands;

      public AllocateSqlDatabaseSharedByServicesToEnvironmentHelper_AllocateSqlDatabaseSharedByServicesToEnvironmentAsync_UnitTests()
      {
         _logger = new Mock<ILogger<ResourceAllocationHelper>>();
         _mockEnvironmentCommands = new Mock<IEnvironmentCommands>();
         _sut = new ResourceAllocationHelper(_logger.Object, _mockEnvironmentCommands.Object);
      }

      [Fact]
      public async Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync_HappyPath_CommandCalledOnce()
      {
         // Arrange
         _mockEnvironmentCommands
            .Setup(m => m.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(() =>
            {
               return Task.CompletedTask;
            });

         // Act
         await _sut.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(Some.Random.Guid());

         // Assert
         _mockEnvironmentCommands.Verify(ec => ec.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
      }
   }
}
