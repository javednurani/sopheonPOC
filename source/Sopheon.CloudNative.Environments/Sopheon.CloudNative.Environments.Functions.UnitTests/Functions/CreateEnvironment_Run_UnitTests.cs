using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Moq;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Functions
{
   public class CreateEnvironment_Run_UnitTests : FunctionUnitTestBase
   {
      private readonly CreateEnvironment Sut;

      public CreateEnvironment_Run_UnitTests()
      {
         _mockEnvironmentRepository.Setup(m => m.AddEnvironment(It.IsAny<Environment>())).Returns((Environment e) =>
         {
            return Task.FromResult(e);
         });

         Sut = new CreateEnvironment(_mockEnvironmentRepository.Object, _mapper, _environmentDtoValidator, _responseBuilder);
      }

      [Fact]
      public async Task Run_HappyPath_ReturnsCreated()
      {
         // Arrange
         EnvironmentDto environmentRequest = new EnvironmentDto
         {
            Name = Some.Random.String(),
            Owner = Some.Random.Guid(),
            Description = Some.Random.String()
         };

         SetRequestBody(_request, environmentRequest);

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.Created, result.StatusCode);

         // EF
         _mockEnvironmentRepository.Verify(m => m.AddEnvironment(It.Is<Environment>(x =>
            x.Name == environmentRequest.Name &&
            x.Owner == environmentRequest.Owner &&
            x.Description == environmentRequest.Description &&
            x.EnvironmentKey == Guid.Empty
         )), Times.Once());

         // HTTP response
         string responseBody = await GetResponseBody(result);
         EnvironmentDto environmentResponse = JsonSerializer.Deserialize<EnvironmentDto>(responseBody);

         Assert.Equal(environmentRequest.Name, environmentResponse.Name);
         Assert.Equal(environmentRequest.Owner, environmentResponse.Owner);
         Assert.Equal(environmentRequest.Description, environmentResponse.Description);
      }

      [Fact]
      public async Task Run_RequestMissingName_ReturnsBadRequest()
      {
         // Arrange
         SetRequestBody(_request,
            new EnvironmentDto
            {
               // Name field is missing
               Owner = Some.Random.Guid(),
               Description = Some.Random.String()
            });

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object);

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         ErrorDto errorResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal($"'{nameof(EnvironmentDto.Name)}' must not be empty.", errorResponse.Message);
      }

      [Fact]
      public async Task Run_RequestMissingOwner_ReturnsBadRequest()
      {
         // Arrange
         SetRequestBody(_request,
            new EnvironmentDto
            {
               Name = Some.Random.String(),
               // Owner field is missing
               Description = Some.Random.String()
            });

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object);

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         ErrorDto errorResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal($"'{nameof(EnvironmentDto.Owner)}' must not be empty.", errorResponse.Message);
      }

      [Fact]
      public async Task Run_OwnerNotValidGuid_ReturnsBadRequest()
      {
         // Arrange
         SetRequestBody(_request,
            // this anonymous object should NOT deserialize to an EnvironmentDto, will instead throw a JsonSerializationException
            new
            {
               Name = Some.Random.String(),
               Owner = "thisIsNotAGuid",
               Description = Some.Random.String()
            });

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object);

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         ErrorDto errorResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal(StringConstants.RESPONSE_REQUEST_BODY_INVALID, errorResponse.Message);
      }
   }
}
