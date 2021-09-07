using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Sopheon.CloudNative.Environments.Domain.Data;
using Sopheon.CloudNative.Environments.Domain.Models;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Functions.UnitTests.TestHelpers;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class CreateEnvironment_Run_UnitTests
   {
      CreateEnvironment Sut;

      Mock<FunctionContext> _context;
      Mock<HttpRequestData> _request;

      Mock<EnvironmentContext> _mockDbContext;
      Mock<DbSet<Environment>> _mockDbSetEnvironments;

      IMapper _mapper;

      public CreateEnvironment_Run_UnitTests()
      {
         TestSetup();
      }

      [Fact]
      public async void Run_HappyPath_ReturnsCreated()
      {
         // Arrange
         SetRequestBody(
            new EnvironmentDto
            {
               Name = SomeRandom.String(),
               Owner = SomeRandom.Guid(),
               Description = SomeRandom.String()
            });

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object);

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         string responseBody = await GetResponseBody(result);
         Assert.Equal($"{nameof(EnvironmentDto.Name)} field is required.", responseBody);
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
         Assert.Equal($"{nameof(EnvironmentDto.Name)} field is required.", responseBody);
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
         Assert.Equal($"{nameof(EnvironmentDto.Owner)} field is required.", responseBody);
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
         _request.CallBase = true;

         _request.Setup(r => r.CreateResponse()).Returns(() =>
         {
            Mock<HttpResponseData> response = new Mock<HttpResponseData>(_context.Object);
            response.CallBase = true;
            response.SetupProperty(r => r.Headers, new HttpHeadersCollection());
            response.SetupProperty(r => r.StatusCode);
            response.SetupProperty(r => r.Body, new MemoryStream());
            //response.Setup(r => r.WriteStringAsync(It.IsAny<string>(), null)).Returns(() => Task.FromResult(typeof(void)));
            //response.Setup(r => r.WriteStringAsync(It.IsAny<string>(), null)).Callback<string>(s => response.Object.Body.Write(Encoding.ASCII.GetBytes(s)));
            return response.Object;
         });

         // DBContext mock
         _mockDbSetEnvironments = new Mock<DbSet<Environment>>();
         _mockDbContext = new Mock<EnvironmentContext>();
         _mockDbContext.Setup(m => m.Environments).Returns(_mockDbSetEnvironments.Object);

         // AutoMapper config
         MapperConfiguration config = new MapperConfiguration(cfg =>
         {
            cfg.AddProfile(new MappingProfile());
         });
         _mapper = config.CreateMapper();

         // create Sut
         Sut = new CreateEnvironment(_mockDbContext.Object, _mapper);
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
