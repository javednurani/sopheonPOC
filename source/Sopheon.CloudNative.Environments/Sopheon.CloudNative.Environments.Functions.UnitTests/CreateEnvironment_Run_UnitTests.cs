using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Azure.Functions.Worker.Http;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Functions.Validators;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class CreateEnvironment_Run_UnitTests : FunctionUnitTestBase
   {
      CreateEnvironment Sut;

      Mock<HttpRequestData> _request;

      Mock<IEnvironmentRepository> _mockEnvironmentRepository;
      HttpResponseDataBuilder _responseBuilder;

      IValidator<EnvironmentDto> _validator;

      public CreateEnvironment_Run_UnitTests()
      {
         TestSetup();
      }

      [Fact]
      public async void Run_HappyPath_ReturnsCreated()
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
      public async void Run_RequestMissingName_ReturnsBadRequest()
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
         ErrorDto exceptionResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal($"'{nameof(EnvironmentDto.Name)}' must not be empty.", exceptionResponse.Message);
      }

      [Fact]
      public async void Run_RequestMissingOwner_ReturnsBadRequest()
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
         ErrorDto exceptionResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal($"'{nameof(EnvironmentDto.Owner)}' must not be empty.", exceptionResponse.Message);
      }

      [Fact]
      public async void Run_OwnerNotValidGuid_ReturnsBadRequest()
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
         ErrorDto exceptionResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal(StringConstants.RESPONSE_REQUEST_BODY_INVALID, exceptionResponse.Message);
      }


      private void TestSetup()
      {
         SetupFunctionContext();

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

         // EnvironmentRepository Mock
         _mockEnvironmentRepository = new Mock<IEnvironmentRepository>();
         _mockEnvironmentRepository.Setup(m => m.AddEnvironment(It.IsAny<Environment>())).Returns((Environment e) =>
         {
            return Task.FromResult(e);
         });

         SetupAutoMapper();

         _validator = new EnvironmentDtoValidator();
         _responseBuilder = new HttpResponseDataBuilder();

         // create Sut
         Sut = new CreateEnvironment(_mockEnvironmentRepository.Object, _mapper, _validator, _responseBuilder);
      }
   }
}
