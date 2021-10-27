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
   public class ResourceAllocatorHelper_AllocateResourcesForEnvironment_UnitTests
   {
      private ResourceAllocatorHelper _sut;
      private Mock<ILogger<ResourceAllocatorHelper>> _logger;
      private Mock<IEnvironmentCommands> _mockEnvironmentCommands;

      public ResourceAllocatorHelper_AllocateResourcesForEnvironment_UnitTests()
      {
         _logger = new Mock<ILogger<ResourceAllocatorHelper>>();
         _mockEnvironmentCommands = new Mock<IEnvironmentCommands>();
         _sut = new ResourceAllocatorHelper(_logger.Object, _mockEnvironmentCommands.Object);
      }

      [Fact]
      public async Task AllocateResourcesForEnvironment_HappyPath_CommandCalledOnce()
      {
         // Arrange
         _mockEnvironmentCommands
            .Setup(m => m.AllocateResourcesForEnvironment(It.IsAny<Guid>()))
            .Returns(() =>
            {
               return Task.CompletedTask;
            });

         // Act
         await _sut.AllocateResourcesForEnvironment(Some.Random.Guid());

         // Assert
         _mockEnvironmentCommands.Verify(ec => ec.AllocateResourcesForEnvironment(It.IsAny<Guid>()), Times.Once); ;
      }
   }
}
