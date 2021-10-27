using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Moq;
using Sopheon.CloudNative.Environments.Functions.Functions;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Functions
{
   public class ResourceAllocator_Run_UnitTests : FunctionUnitTestBase
   {
      private readonly ResourceAllocator _sut;
      private readonly Mock<IResourceAllocatorHelper> _mockAllocatorHelper;

      public ResourceAllocator_Run_UnitTests()
      {
         _mockAllocatorHelper = new Mock<IResourceAllocatorHelper>();
         _sut = new ResourceAllocator(_mockAllocatorHelper.Object, _responseBuilder);
      }

      [Fact]
      public async Task Run_HappyPath_CallsHelperOnce()
      {
         // Arrange

         // Act
         HttpResponseData result = await _sut.Run(_request.Object, _context.Object, Some.Random.Guid().ToString());

         // Assert
         Assert.NotNull(result);
         _mockAllocatorHelper.Verify(mh => mh.AllocateResourcesForEnvironment(It.IsAny<Guid>()), Times.Once);
      }
   }
}
