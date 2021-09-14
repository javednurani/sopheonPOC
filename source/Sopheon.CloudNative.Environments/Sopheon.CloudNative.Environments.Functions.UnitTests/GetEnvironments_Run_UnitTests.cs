using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Testing.Common;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class GetEnvironments_Run_UnitTests : FunctionUnitTestBase
   {
      GetEnvironments Sut;

      Mock<FunctionContext> _context;
      Mock<HttpRequestData> _request;

      Mock<IEnvironmentRepository> _mockEnvironmentRepository;

      IMapper _mapper;

      public GetEnvironments_Run_UnitTests()
      {
         TestSetup();
      }

      [Fact]
      public async void Run_HappyPath_EnvironmentsReturned()
      {
         // Arrange
         _mockEnvironmentRepository.Setup(m => m.GetEnvironments()).Returns(() =>
         {
            IEnumerable<Environment> environments = new List<Environment>
            {
               new Environment
               {
                  Name = Some.Random.String(),
                  Owner = Some.Random.Guid(),
                  EnvironmentKey = Some.Random.Guid(),
                  Description = Some.Random.String(),
                  IsDeleted = false,
               },
               new Environment
               {
                  Name = Some.Random.String(),
                  Owner = Some.Random.Guid(),
                  EnvironmentKey = Some.Random.Guid(),
                  Description = Some.Random.String(),
                  IsDeleted = false,
               },
            };
            return Task.FromResult(environments);
         });
         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.OK, result.StatusCode);

         // EF
         _mockEnvironmentRepository.Verify(m => m.GetEnvironments(), Times.Once());

         // HTTP response
         string responseBody = await GetResponseBody(result);
         List<EnvironmentDto> environmentResponse = JsonSerializer.Deserialize<List<EnvironmentDto>>(responseBody);

         Assert.NotEmpty(environmentResponse);
      }

      [Fact]
      public async void Run_HappyPath_NoneReturned()
      {
         // Arrange
         _mockEnvironmentRepository.Setup(m => m.GetEnvironments()).Returns(() =>
         {
            IEnumerable<Environment> environments = new List<Environment>();
            return Task.FromResult(environments);
         });

         // Act
         HttpResponseData result = await Sut.Run(_request.Object, _context.Object);
         result.Body.Position = 0;

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.OK, result.StatusCode);

         // EF
         _mockEnvironmentRepository.Verify(m => m.GetEnvironments(), Times.Once());

         // HTTP response
         string responseBody = await GetResponseBody(result);
         List<EnvironmentDto> environmentResponse = JsonSerializer.Deserialize<List<EnvironmentDto>>(responseBody);
         Assert.Empty(environmentResponse);
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

         // AutoMapper config
         MapperConfiguration config = new MapperConfiguration(cfg =>
         {
            cfg.AddProfile(new MappingProfile());
         });
         _mapper = config.CreateMapper();

         // create Sut
         Sut = new GetEnvironments(_mockEnvironmentRepository.Object, _mapper);
      }
   }
}
