using AutoMapper;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Functions.Validators;
using Sopheon.CloudNative.Environments.Testing.Common;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;


namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class UpdateEnvironment_Run_UnitTests
   {
      UpdateEnvironment Sut;

      Mock<FunctionContext> _context;
      Mock<HttpRequestData> _request;

      Mock<IEnvironmentRepository> _mockEnvironmentRepository;
      HttpResponseDataBuilder _responseBuilder;

      IMapper _mapper;
      IValidator<EnvironmentDto> _validator;

      public UpdateEnvironment_Run_UnitTests()
      {
         TestSetup();
      }

      [Fact]
      public async void Run_HappyPath_ReturnsOK()
      {
         Guid environmentKey = SomeRandom.Guid();
         // Arrange
         EnvironmentDto environmentRequest = new EnvironmentDto
         {
            Name = SomeRandom.String(),
            Owner = SomeRandom.Guid(),
            Description = SomeRandom.String()
         };

         SetRequestBody(environmentRequest);

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
         EnvironmentDto environmentResponse = JsonConvert.DeserializeObject<EnvironmentDto>(responseBody);

         Assert.Equal(environmentRequest.Name, environmentResponse.Name);
         Assert.Equal(environmentRequest.Owner, environmentResponse.Owner);
         Assert.Equal(environmentRequest.Description, environmentResponse.Description);
      }

      [Fact]
      public async void Run_NonGuidKey_ReturnsBadRequest()
      {
         // Arrange
         string environmentKey = SomeRandom.String();
         EnvironmentDto environmentRequest = new EnvironmentDto
         {
            Name = SomeRandom.String(),
            Owner = SomeRandom.Guid(),
            Description = SomeRandom.String()
         };

         SetRequestBody(environmentRequest);

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, environmentKey.ToString());
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);       

         // HTTP response
         string responseBody = await GetResponseBody(result);

         Assert.Equal("The EnvironmentKey must be a valid Guid", responseBody);        
      }

      [Fact]
      public async void Run_NameMissing_ReturnsBadRequest()
      {
         // Arrange         
         Guid environmentKey = SomeRandom.Guid();
         EnvironmentDto environmentRequest = new EnvironmentDto
         {
            Owner = SomeRandom.Guid(),
            Description = SomeRandom.String()
         };

         SetRequestBody(environmentRequest);

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, environmentKey.ToString());
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         // HTTP response
         string responseBody = await GetResponseBody(result);

         Assert.Equal("'Name' must not be empty.", responseBody);
      }

      [Fact]
      public async void Run_OwnerMissing_ReturnsBadRequest()
      {
         // Arrange         
         Guid environmentKey = SomeRandom.Guid();
         EnvironmentDto environmentRequest = new EnvironmentDto
         {
            Name = SomeRandom.String(),            
            Description = SomeRandom.String()
         };

         SetRequestBody(environmentRequest);

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, environmentKey.ToString());
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         // HTTP response
         string responseBody = await GetResponseBody(result);

         Assert.Equal("'Owner' must not be empty.", responseBody);
      }

      [Fact]
      public async void Run_EnvironmentDoesNotExist_ReturnsBadRequest()
      {
         // Arrange         
         Guid environmentKey = SomeRandom.Guid();
         EnvironmentDto environmentRequest = new EnvironmentDto
         {
            Name = SomeRandom.String(),
            Description = SomeRandom.String(),
            Owner = SomeRandom.Guid(),
         };

         SetRequestBody(environmentRequest);
         string mockExceptionMessage = SomeRandom.String();
         _mockEnvironmentRepository.Setup(er => er.UpdateEnvironment(It.IsAny<Environment>())).Throws(new EntityNotFoundException(mockExceptionMessage));

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object, environmentKey.ToString());
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
         string responseBody = await GetResponseBody(result);
         Assert.Equal(mockExceptionMessage, responseBody);
      }

      //TODO: Different mock to return not found and test EntityNotFoundException? Is this valuable to us? Better in a repository unit test?

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

         // EnvironmentRepository Mock
         _mockEnvironmentRepository = new Mock<IEnvironmentRepository>();
         _mockEnvironmentRepository.Setup(m => m.UpdateEnvironment(It.IsAny<Environment>())).Returns((Environment e) =>
         {
            return Task.FromResult(e);
         });

         // AutoMapper config
         MapperConfiguration config = new MapperConfiguration(cfg =>
         {
            cfg.AddProfile(new MappingProfile());
         });
         _mapper = config.CreateMapper();

         _validator = new EnvironmentDtoValidator();
         _responseBuilder = new HttpResponseDataBuilder();

         // create Sut
         Sut = new UpdateEnvironment(_mockEnvironmentRepository.Object, _mapper, _validator, _responseBuilder);
      }

      private async Task<string> GetResponseBody(HttpResponseData response)
      {
         response.Body.Position = 0;
         StreamReader reader = new StreamReader(response.Body);
         return await reader.ReadToEndAsync();
      }

      private void SetRequestBody(object requestObject)
      {
         byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(requestObject));
         MemoryStream bodyStream = new MemoryStream(byteArray);

         _request.Setup(r => r.Body).Returns(bodyStream);
      }
   }
}
