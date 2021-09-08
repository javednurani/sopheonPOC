using AutoMapper;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Functions.UnitTests.TestHelpers;
using Sopheon.CloudNative.Environments.Functions.Validators;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class CreateEnvironment_Run_UnitTests
   {
      CreateEnvironment Sut;

      Mock<FunctionContext> _context;
      Mock<HttpRequestData> _request;

      Mock<IEnvironmentRepository> _mockEnvironmentRepository;

      IMapper _mapper;
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
            Name = SomeRandom.String(),
            Owner = SomeRandom.Guid(),
            Description = SomeRandom.String()
         };

         SetRequestBody(environmentRequest);

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
         EnvironmentDto environmentResponse = JsonConvert.DeserializeObject<EnvironmentDto>(responseBody);

         Assert.Equal(environmentRequest.Name, environmentResponse.Name);
         Assert.Equal(environmentRequest.Owner, environmentResponse.Owner);
         Assert.Equal(environmentRequest.Description, environmentResponse.Description);
      }

      [Fact]
      public async void Run_RequestMissingName_ReturnsBadRequest()
      {
         // Arrange
         SetRequestBody(
            new EnvironmentDto
            {
               // Name field is missing
               Owner = SomeRandom.Guid(),
               Description = SomeRandom.String()
            });

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object);

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         Assert.Equal($"'{nameof(EnvironmentDto.Name)}' must not be empty.", responseBody);
      }

      [Fact]
      public async void Run_RequestMissingOwner_ReturnsBadRequest()
      {
         // Arrange
         SetRequestBody(
            new EnvironmentDto
            {
               Name = SomeRandom.String(),
               // Owner field is missing
               Description = SomeRandom.String()
            });

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object);

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         Assert.Equal($"'{nameof(EnvironmentDto.Owner)}' must not be empty.", responseBody);
      }

      [Fact]
      public async void Run_OwnerNotValidGuid_ReturnsBadRequest()
      {
         // Arrange
         SetRequestBody(
            // this anonymous object should NOT deserialize to an EnvironmentDto, will instead throw a JsonSerializationException
            new
            {
               Name = SomeRandom.String(),
               Owner = "thisIsNotAGuid",
               Description = SomeRandom.String()
            });

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object);

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         Assert.Equal($"Request body was invalid. Is {nameof(EnvironmentDto.Owner)} field a valid GUID?", responseBody);
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

         // EnvironmentRepository Mock
         _mockEnvironmentRepository = new Mock<IEnvironmentRepository>();
         _mockEnvironmentRepository.Setup(m => m.AddEnvironment(It.IsAny<Environment>())).Returns((Environment e) => {
            return Task.FromResult(e);
         });

         // AutoMapper config
         MapperConfiguration config = new MapperConfiguration(cfg =>
         {
            cfg.AddProfile(new MappingProfile());
         });
         _mapper = config.CreateMapper();

         _validator = new EnvironmentDtoValidator();

         // create Sut
         Sut = new CreateEnvironment(_mockEnvironmentRepository.Object, _mapper, _validator);
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
