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
   public class AllocateSqlDatabaseSharedByServicesToEnvironment_Run_UnitTests : FunctionUnitTestBase
   {
      private readonly AllocateSqlDatabaseSharedByServicesToEnvironment _sut;
      private readonly Mock<IResourceAllocationHelper> _mockAllocatorHelper;

      public AllocateSqlDatabaseSharedByServicesToEnvironment_Run_UnitTests()
      {
         _mockAllocatorHelper = new Mock<IResourceAllocationHelper>();
         _sut = new AllocateSqlDatabaseSharedByServicesToEnvironment(_mockAllocatorHelper.Object, _responseBuilder);
      }

      [Fact]
      public async Task Run_HappyPath_CallsHelperOnce()
      {
         // Arrange

         // Act
         HttpResponseData result = await _sut.Run(_request.Object, _context.Object, Some.Random.Guid());

         // Assert
         Assert.NotNull(result);
         _mockAllocatorHelper.Verify(mh => mh.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(It.IsAny<Guid>()), Times.Once);
      }
   }
}
