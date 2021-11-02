using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;


namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Functions
{
   public class UpdateEnvironment_Run_UnitTests : FunctionUnitTestBase
   {
      private readonly UpdateEnvironment Sut;

      public UpdateEnvironment_Run_UnitTests()
      {
         _mockEnvironmentRepository.Setup(m => m.UpdateEnvironment(It.IsAny<Environment>())).Returns((Environment e) =>
         {
            return Task.FromResult(e);
         });

         // create Sut
         Sut = new UpdateEnvironment(_mockEnvironmentRepository.Object, _mapper, _environmentDtoValidator, _responseBuilder);
      }

      [Fact]
      public async Task Run_HappyPath_ReturnsOK()
      {
         Guid environmentKey = Some.Random.Guid();
         // Arrange
         EnvironmentDto environmentRequest = new EnvironmentDto
         {
            Name = Some.Random.String(),
            Owner = Some.Random.Guid(),
            Description = Some.Random.String()
         };

         SetRequestBody(_request, environmentRequest);

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, environmentKey);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.OK, result.StatusCode);

         // EF
         _mockEnvironmentRepository.Verify(m => m.UpdateEnvironment(It.Is<Environment>(x =>
            x.Name == environmentRequest.Name &&
            x.Owner == environmentRequest.Owner &&
            x.Description == environmentRequest.Description &&
            x.EnvironmentKey == environmentKey
         )), Times.Once());

         // HTTP response
         string responseBody = await GetResponseBody(result);
         EnvironmentDto environmentResponse = JsonSerializer.Deserialize<EnvironmentDto>(responseBody);

         Assert.Equal(environmentRequest.Name, environmentResponse.Name);
         Assert.Equal(environmentRequest.Owner, environmentResponse.Owner);
         Assert.Equal(environmentRequest.Description, environmentResponse.Description);
      }

      [Fact]
      public async Task Run_EmptyEnvironmentKey_ReturnsBadRequest()
      {
         // Arrange
         Guid environmentKey = Guid.Empty;
         EnvironmentDto environmentRequest = new EnvironmentDto() { Name = Some.Random.String(), Owner = Some.Random.Guid(), Description = Some.Random.String() };

         SetRequestBody(_request, environmentRequest);

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, environmentKey);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         // HTTP response
         string responseBody = await GetResponseBody(result);
         ErrorDto errorResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal("The EnvironmentKey must be a valid Guid", errorResponse.Message);
      }

      [Fact]
      public async Task Run_NameMissing_ReturnsBadRequest()
      {
         // Arrange         
         Guid environmentKey = Some.Random.Guid();
         EnvironmentDto environmentRequest = new EnvironmentDto
         {
            Owner = Some.Random.Guid(),
            Description = Some.Random.String()
         };

         SetRequestBody(_request, environmentRequest);

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, environmentKey);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         // HTTP response
         string responseBody = await GetResponseBody(result);
         ErrorDto errorResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal("'Name' must not be empty.", errorResponse.Message);
      }

      [Fact]
      public async Task Run_OwnerMissing_ReturnsBadRequest()
      {
         // Arrange         
         Guid environmentKey = Some.Random.Guid();
         EnvironmentDto environmentRequest = new EnvironmentDto
         {
            Name = Some.Random.String(),
            Description = Some.Random.String()
         };

         SetRequestBody(_request, environmentRequest);

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, environmentKey);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         // HTTP response
         string responseBody = await GetResponseBody(result);
         ErrorDto errorResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal("'Owner' must not be empty.", errorResponse.Message);
      }

      [Fact]
      public async Task Run_EnvironmentDoesNotExist_ReturnsBadRequest()
      {
         // Arrange         
         Guid environmentKey = Some.Random.Guid();
         EnvironmentDto environmentRequest = new EnvironmentDto 
         { 
            Name = Some.Random.String(), 
            Description = Some.Random.String(), 
            Owner = Some.Random.Guid() 
         };

         SetRequestBody(_request, environmentRequest);
         string mockExceptionMessage = Some.Random.String();
         _mockEnvironmentRepository.Setup(er => er.UpdateEnvironment(It.IsAny<Environment>())).Throws(new EntityNotFoundException(mockExceptionMessage));

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, environmentKey);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
         string responseBody = await GetResponseBody(result);
         ErrorDto errorResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal(mockExceptionMessage, errorResponse.Message);
      }
   }
}
