using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Functions
{
   public class DeleteEnvironment_Run_UnitTests : FunctionUnitTestBase
   {
      DeleteEnvironment Sut;

      Mock<IEnvironmentRepository> _mockEnvironmentRepository;
      HttpResponseDataBuilder _responseBuilder;

      public DeleteEnvironment_Run_UnitTests()
      {
         TestSetup();
      }

      [Fact]
      public async void Run_HappyPath_ReturnsNoContent()
      {
         // Arrange
         _mockEnvironmentRepository.Setup(m => m.DeleteEnvironment(It.IsAny<Guid>())).Returns(() =>
         {
            return Task.CompletedTask;
         });

         string keyToDelete = Guid.NewGuid().ToString();

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, keyToDelete);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

         // EF
         _mockEnvironmentRepository.Verify(m => m.DeleteEnvironment(It.Is<Guid>(x =>
            x.ToString() == keyToDelete
         )), Times.Once());
      }

      [Fact]
      public async void Run_KeyIsInvalidGuid_ReturnsBadRequest()
      {
         // Arrange
         string keyToDelete = Some.Random.String();

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, keyToDelete);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
      }

      [Fact]
      public async void Run_EntityNotFound_ReturnsNotFound()
      {
         // Arrange
         _mockEnvironmentRepository
            .Setup(m => m.DeleteEnvironment(It.IsAny<Guid>()))
            .Throws(new EntityNotFoundException());

         string keyToDelete = Guid.NewGuid().ToString();

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, keyToDelete);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

         // EF
         _mockEnvironmentRepository.Verify(m => m.DeleteEnvironment(It.Is<Guid>(x =>
            x.ToString() == keyToDelete
         )), Times.Once());
      }

      [Fact]
      public async void Run_GeneralException_ReturnsServerError()
      {
         // Arrange
         _mockEnvironmentRepository
            .Setup(m => m.DeleteEnvironment(It.IsAny<Guid>()))
            .Throws(new Exception());

         string keyToDelete = Guid.NewGuid().ToString();

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, keyToDelete);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);

         // EF
         _mockEnvironmentRepository.Verify(m => m.DeleteEnvironment(It.Is<Guid>(x =>
            x.ToString() == keyToDelete
         )), Times.Once());
      }

      private void TestSetup()
      {
         _mockEnvironmentRepository = new Mock<IEnvironmentRepository>();

         _responseBuilder = new HttpResponseDataBuilder();

         // create Sut
         Sut = new DeleteEnvironment(_mockEnvironmentRepository.Object, _responseBuilder);
      }
   }
}
