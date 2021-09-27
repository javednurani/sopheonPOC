﻿using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Azure.Functions.Worker.Http;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Functions.Validators;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;


namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class UpdateEnvironment_Run_UnitTests : FunctionUnitTestBase
   {
      UpdateEnvironment Sut;

      Mock<HttpRequestData> _request;

      Mock<IEnvironmentRepository> _mockEnvironmentRepository;
      HttpResponseDataBuilder _responseBuilder;

      IValidator<EnvironmentDto> _validator;

      public UpdateEnvironment_Run_UnitTests()
      {
         TestSetup();
      }

      [Fact]
      public async void Run_HappyPath_ReturnsOK()
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
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, environmentKey.ToString());
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
      public async void Run_NonGuidKey_ReturnsBadRequest()
      {
         // Arrange
         string environmentKey = Some.Random.String();
         EnvironmentDto environmentRequest = new EnvironmentDto
         {
            Name = Some.Random.String(),
            Owner = Some.Random.Guid(),
            Description = Some.Random.String()
         };

         SetRequestBody(_request, environmentRequest);

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, environmentKey.ToString());
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         // HTTP response
         string responseBody = await GetResponseBody(result);
         ErrorDto exceptionResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal("The EnvironmentKey must be a valid Guid", exceptionResponse.Message);
      }

      [Fact]
      public async void Run_NameMissing_ReturnsBadRequest()
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
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, environmentKey.ToString());
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         // HTTP response
         string responseBody = await GetResponseBody(result);
         ErrorDto exceptionResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal("'Name' must not be empty.", exceptionResponse.Message);
      }

      [Fact]
      public async void Run_OwnerMissing_ReturnsBadRequest()
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
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, environmentKey.ToString());
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         // HTTP response
         string responseBody = await GetResponseBody(result);
         ErrorDto exceptionResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal("'Owner' must not be empty.", exceptionResponse.Message);
      }

      [Fact]
      public async void Run_EnvironmentDoesNotExist_ReturnsBadRequest()
      {
         // Arrange         
         Guid environmentKey = Some.Random.Guid();
         EnvironmentDto environmentRequest = new EnvironmentDto
         {
            Name = Some.Random.String(),
            Description = Some.Random.String(),
            Owner = Some.Random.Guid(),
         };

         SetRequestBody(_request, environmentRequest);
         string mockExceptionMessage = Some.Random.String();
         _mockEnvironmentRepository.Setup(er => er.UpdateEnvironment(It.IsAny<Environment>())).Throws(new EntityNotFoundException(mockExceptionMessage));

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, environmentKey.ToString());
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
         string responseBody = await GetResponseBody(result);
         ErrorDto exceptionResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal(mockExceptionMessage, exceptionResponse.Message);
      }

      //TODO: Different mock to return not found and test EntityNotFoundException? Is this valuable to us? Better in a repository unit test?

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
         _mockEnvironmentRepository.Setup(m => m.UpdateEnvironment(It.IsAny<Environment>())).Returns((Environment e) =>
         {
            return Task.FromResult(e);
         });

         SetupAutoMapper();

         _validator = new EnvironmentDtoValidator();
         _responseBuilder = new HttpResponseDataBuilder();

         // create Sut
         Sut = new UpdateEnvironment(_mockEnvironmentRepository.Object, _mapper, _validator, _responseBuilder);
      }
   }
}
