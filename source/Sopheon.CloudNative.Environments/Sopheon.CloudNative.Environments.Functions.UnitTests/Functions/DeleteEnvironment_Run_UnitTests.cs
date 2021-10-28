using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Functions
{
   public class DeleteEnvironment_Run_UnitTests : FunctionUnitTestBase
   {
      DeleteEnvironment Sut;

      public DeleteEnvironment_Run_UnitTests()
      {
         Sut = new DeleteEnvironment(_mockEnvironmentRepository.Object, _responseBuilder);
      }

      [Fact]
      public async Task Run_HappyPath_ReturnsNoContent()
      {
         // Arrange
         _mockEnvironmentRepository.Setup(m => m.DeleteEnvironment(It.IsAny<Guid>())).Returns(() =>
         {
            return Task.CompletedTask;
         });

         Guid keyToDelete = Guid.NewGuid();

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, keyToDelete);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

         // EF
         _mockEnvironmentRepository.Verify(m => m.DeleteEnvironment(It.Is<Guid>(x =>
            x == keyToDelete
         )), Times.Once());
      }

      [Fact]
      public async Task Run_EmptyEnvironmentKey_ReturnsBadRequest()
      {
         // Arrange
         Guid keyToDelete = Guid.Empty;

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, keyToDelete);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
      }

      [Fact]
      public async Task Run_EntityNotFound_ReturnsNotFound()
      {
         // Arrange
         _mockEnvironmentRepository
            .Setup(m => m.DeleteEnvironment(It.IsAny<Guid>()))
            .Throws(new EntityNotFoundException());

         Guid keyToDelete = Guid.NewGuid();

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, keyToDelete);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

         // EF
         _mockEnvironmentRepository.Verify(m => m.DeleteEnvironment(It.Is<Guid>(x =>
            x == keyToDelete
         )), Times.Once());
      }

      [Fact]
      public async Task Run_GeneralException_ReturnsServerError()
      {
         // Arrange
         _mockEnvironmentRepository
            .Setup(m => m.DeleteEnvironment(It.IsAny<Guid>()))
            .Throws(new Exception());

         Guid keyToDelete = Guid.NewGuid();

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, keyToDelete);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);

         // EF
         _mockEnvironmentRepository.Verify(m => m.DeleteEnvironment(It.Is<Guid>(x =>
            x == keyToDelete
         )), Times.Once());
      }
   }
}
