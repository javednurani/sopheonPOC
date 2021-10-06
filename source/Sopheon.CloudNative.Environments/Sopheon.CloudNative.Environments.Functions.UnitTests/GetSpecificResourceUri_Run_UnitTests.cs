using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Queries;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class GetSpecificResourceUri_Run_UnitTests : FunctionUnitTestBase
   {
      GetSpecificResourceUri Sut;

      Mock<HttpRequestData> _request;
      Mock<IEnvironmentQueries> _mockEnvironmentQueries;
      HttpResponseDataBuilder _responseBuilder;

      public GetSpecificResourceUri_Run_UnitTests()
      {
         TestSetup();
      }

      [Fact]
      public async void Run_HappyPath_UriReturned()
      {
         // Arrange
         Guid testGuid = Some.Random.Guid();
         _mockEnvironmentQueries
            .Setup(m => m.GetSpecificResourceUri(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(() =>
            {
               string resourceUri = "someUri1";
               return Task.FromResult(resourceUri);
            });

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, testGuid, "asdf", "asdf");
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.OK, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         ResourceUriDto resourceUriResponse = JsonSerializer.Deserialize<ResourceUriDto>(responseBody);  

         Assert.NotEmpty(resourceUriResponse.Uri);

         _mockEnvironmentQueries.Verify(m => m.GetSpecificResourceUri(testGuid, "asdf", "asdf"), Times.Once());
      }

      [Fact]
      public async void Run_EnvironmentKeyIsEmptyGuid_BadRequest()
      {
         // Arrange + Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, Guid.Empty, Some.Random.String(), Some.Random.String());
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         ErrorDto errorResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal(StringConstants.RESPONSE_REQUEST_PATH_PARAMETER_INVALID, errorResponse.Message);

         _mockEnvironmentQueries.Verify(m => m.GetSpecificResourceUri(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
      }

      [Fact]
      public async void Run_BusinessServiceNameIsEmptyString_BadRequest()
      {
         // Arrange + Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, Some.Random.Guid(), string.Empty, Some.Random.String());
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         ErrorDto errorResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal(StringConstants.RESPONSE_REQUEST_PATH_PARAMETER_INVALID, errorResponse.Message);

         _mockEnvironmentQueries.Verify(m => m.GetSpecificResourceUri(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
      }

      [Fact]
      public async void Run_DependencyNameIsEmptyString_BadRequest()
      {
         // Arrange + Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, Some.Random.Guid(), Some.Random.String(), string.Empty);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         ErrorDto errorResponse = JsonSerializer.Deserialize<ErrorDto>(responseBody);

         Assert.Equal(StringConstants.RESPONSE_REQUEST_PATH_PARAMETER_INVALID, errorResponse.Message);

         _mockEnvironmentQueries.Verify(m => m.GetSpecificResourceUri(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
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

         _mockEnvironmentQueries = new Mock<IEnvironmentQueries>();

         // AutoMapper config
         MapperConfiguration config = new MapperConfiguration(cfg =>
         {
            cfg.AddProfile(new MappingProfile());
         });
         _mapper = config.CreateMapper();

         _responseBuilder = new HttpResponseDataBuilder();

         // create Sut
         Sut = new GetSpecificResourceUri(_mockEnvironmentQueries.Object, _mapper, _responseBuilder);
      }
   }
}
