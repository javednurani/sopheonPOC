using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.UnitTests.TestHelpers;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class DeleteEnvironment_Run_UnitTests
   {
      DeleteEnvironment Sut;

      Mock<FunctionContext> _context;
      Mock<HttpRequestData> _request;

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
         _mockEnvironmentRepository.Setup(m => m.DeleteEnvironment(It.IsAny<Environment>())).Returns(() =>
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
         _mockEnvironmentRepository.Verify(m => m.DeleteEnvironment(It.Is<Environment>(x =>
            x.EnvironmentKey.ToString() == keyToDelete
         )), Times.Once());
      }

      [Fact]
      public async void Run_KeyIsInvalidGuid_ReturnsBadRequest()
      {
         // Arrange
         string keyToDelete = SomeRandom.String();

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
            .Setup(m => m.DeleteEnvironment(It.IsAny<Environment>()))
            .Throws(new EntityNotFoundException());

         string keyToDelete = Guid.NewGuid().ToString();

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, keyToDelete);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

         // EF
         _mockEnvironmentRepository.Verify(m => m.DeleteEnvironment(It.Is<Environment>(x =>
            x.EnvironmentKey.ToString() == keyToDelete
         )), Times.Once());
      }

      [Fact]
      public async void Run_GeneralException_ReturnsServerError()
      {
         // Arrange
         _mockEnvironmentRepository
            .Setup(m => m.DeleteEnvironment(It.IsAny<Environment>()))
            .Throws(new Exception());

         string keyToDelete = Guid.NewGuid().ToString();

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, keyToDelete);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);

         // EF
         _mockEnvironmentRepository.Verify(m => m.DeleteEnvironment(It.Is<Environment>(x =>
            x.EnvironmentKey.ToString() == keyToDelete
         )), Times.Once());
      }

      private void TestSetup()
      {
         // FunctionContext
         ServiceCollection serviceCollection = new ServiceCollection();
         serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();
         ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

         _context = new Mock<FunctionContext>();
         _context.SetupProperty(c => c.InstanceServices, serviceProvider);

         // HttpRequestData
         _request = new Mock<HttpRequestData>(_context.Object);

         _request.Setup(r => r.CreateResponse()).Returns(() =>
         {
            Mock<HttpResponseData> response = new Mock<HttpResponseData>(_context.Object);
            response.SetupProperty(r => r.Headers, new HttpHeadersCollection());
            response.SetupProperty(r => r.StatusCode);
            response.SetupProperty(r => r.Body, new MemoryStream());
            return response.Object;
         });

         _mockEnvironmentRepository = new Mock<IEnvironmentRepository>();

         _responseBuilder = new HttpResponseDataBuilder();

         // create Sut
         Sut = new DeleteEnvironment(_mockEnvironmentRepository.Object, _responseBuilder);
      }
   }
}
