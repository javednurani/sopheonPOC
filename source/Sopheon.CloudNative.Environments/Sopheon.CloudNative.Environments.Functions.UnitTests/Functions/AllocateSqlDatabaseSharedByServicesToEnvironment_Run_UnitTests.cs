using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Moq;
using Sopheon.CloudNative.Environments.Functions.Functions;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;
using System.Text.Json;
using Sopheon.CloudNative.Environments.Functions.Models;

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
      public async Task Run_HappyPath_ReturnsResourceAllocationResponseDto()
      {
         // Arrange

         // Act
         HttpResponseData result = await _sut.Run(_request.Object, _context.Object, Some.Random.Guid());

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.Created, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         ResourceAllocationResponseDto response = JsonSerializer.Deserialize<ResourceAllocationResponseDto>(responseBody);
         Assert.NotNull(response);
      }

      [Fact]
      public async Task Run_HappyPath_CallsHelperWithCorrectEnvironmentKey()
      {
         // Arrange

         // Act
         Guid environmentKey = Some.Random.Guid();
         HttpResponseData result = await _sut.Run(_request.Object, _context.Object, environmentKey);

         // Assert
         _mockAllocatorHelper.Verify(mh => mh.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(environmentKey, null, null, null), Times.Once);
      }

      [Fact]
      public async Task Run_EmptyEnvironment_ReturnsErrorDto()
      {
         // Arrange

         // Act
         HttpResponseData result = await _sut.Run(_request.Object, _context.Object, Guid.Empty);

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         ErrorDto errorResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);
         Assert.Equal(StringConstants.RESPONSE_REQUEST_ENVIRONMENTKEY_INVALID, errorResponse.Message);
      }

      [Fact]
      public async Task Run_DependencyThrowsException_ReturnsErrorDto()
      {
         // Arrange
         _mockAllocatorHelper
            .Setup(mh => mh.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(It.IsAny<Guid>(), null, null, null))
            .ThrowsAsync(new Exception());

         // Act
         HttpResponseData result = await _sut.Run(_request.Object, _context.Object, Some.Random.Guid());

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         ErrorDto errorResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);
         Assert.Equal(StringConstants.RESPONSE_GENERIC_ERROR, errorResponse.Message);
      }
   }
}
