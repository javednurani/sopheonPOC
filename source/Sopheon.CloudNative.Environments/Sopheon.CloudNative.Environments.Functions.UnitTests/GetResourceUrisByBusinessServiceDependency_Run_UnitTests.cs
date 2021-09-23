using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Queries;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Validators;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class GetResourceUrisByBusinessServiceDependency_Run_UnitTests : FunctionUnitTestBase
   {
      GetResourceUrisByBusinessServiceDependency Sut;

      Mock<HttpRequestData> _request;

      Mock<IEnvironmentQueries> _mockEnvironmentQueries;
      IRequiredNameValidator _validator;
      HttpResponseDataBuilder _responseBuilder;

      public GetResourceUrisByBusinessServiceDependency_Run_UnitTests()
      {
         TestSetup();
      }

      [Fact]
      public async void Run_HappyPath_UrisReturned()
      {
         // Arrange
         string businessServiceName = Some.Random.String();
         string dependencyName = Some.Random.String();

         IEnumerable<string> resourceUris = new List<string>
         {
            Some.Random.String(),
            Some.Random.String(),
            Some.Random.String(),
         };

         _mockEnvironmentQueries
            .Setup(m => m.GetResourceUrisByBusinessServiceDependency(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(() =>
            {
               return Task.FromResult(resourceUris);
            });

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, businessServiceName, dependencyName);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.OK, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         List<string> resourceUriResponse = JsonSerializer.Deserialize<List<string>>(responseBody);

         Assert.NotEmpty(resourceUriResponse);
         Assert.Equal(resourceUris, resourceUriResponse);

         _mockEnvironmentQueries.Verify(m => m.GetResourceUrisByBusinessServiceDependency(businessServiceName, dependencyName), Times.Once());
      }

      [Fact]
      public async void Run_BusinessServiceKeyIsEmptyString_BadRequest()
      {
         // Arrange + Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, string.Empty, Some.Random.String());
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         Assert.Equal(StringConstants.RESPONSE_REQUEST_PATH_PARAMETER_INVALID, responseBody);

         _mockEnvironmentQueries.Verify(m => m.GetResourceUrisByBusinessServiceDependency(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
      }

      [Fact]
      public async void Run_DependencyKeyIsEmptyString_BadRequest()
      {
         // Arrange + Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, Some.Random.String(), string.Empty);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         Assert.Equal(StringConstants.RESPONSE_REQUEST_PATH_PARAMETER_INVALID, responseBody);

         _mockEnvironmentQueries.Verify(m => m.GetResourceUrisByBusinessServiceDependency(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
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

         _mockEnvironmentQueries = new Mock<IEnvironmentQueries>();

         SetupAutoMapper();

         _validator = new RequiredNameValidator();
         _responseBuilder = new HttpResponseDataBuilder();

         // create Sut
         Sut = new GetResourceUrisByBusinessServiceDependency(_mockEnvironmentQueries.Object, _validator, _mapper, _responseBuilder);
      }
   }
}
