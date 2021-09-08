using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class GetEnvironmentst_Run_UnitTests
   {
      GetEnvironments Sut;

      Mock<FunctionContext> _context;
      Mock<HttpRequestData> _request;

      Mock<IEnvironmentRepository> _mockEnvironmentRepository;

      IMapper _mapper;

      public GetEnvironmentst_Run_UnitTests()
      {
         TestSetup();
      }

      [Fact]
      public async void Run_HappyPath_EnvironmentsReturned()
      {
         // Arrange
         _mockEnvironmentRepository.Setup(m => m.GetEnvironments()).Returns(() => {
            List<Environment> environments = new List<Environment>
            {
               new Environment
               {
                  Name = "NotDeleted",
                  Owner = new System.Guid(),
                  EnvironmentKey = new System.Guid(),
                  Description = "",
                  IsDeleted = false,
               },
               new Environment
               {
                  Name = "Deleted",
                  Owner = new System.Guid(),
                  EnvironmentKey = new System.Guid(),
                  Description = "",
                  IsDeleted = true,
               }
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
         List<EnvironmentDto> environmentResponse = JsonConvert.DeserializeObject<List<EnvironmentDto>>(responseBody);

         Assert.Single(environmentResponse);
      }

      [Fact]
      public async void Run_HappyPath_NoDeletedReturned()
      {
         // Arrange
         _mockEnvironmentRepository.Setup(m => m.GetEnvironments()).Returns(() => {
            List<Environment> environments = new List<Environment>
            {
               new Environment
               {
                  Name = "Deleted",
                  Owner = new System.Guid(),
                  EnvironmentKey = new System.Guid(),
                  Description = "",
                  IsDeleted = true,
               }
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
         List<EnvironmentDto> environmentResponse = JsonConvert.DeserializeObject<List<EnvironmentDto>>(responseBody);
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

      private async Task<string> GetResponseBody(HttpResponseData response)
      {
         response.Body.Position = 0;
         StreamReader reader = new StreamReader(response.Body);
         return await reader.ReadToEndAsync();
      }
   }
}
