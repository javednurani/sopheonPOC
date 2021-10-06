using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Repositories;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Functions
{
   public class FunctionUnitTestBase
   {
      // TODO: rename casing
      protected Mock<FunctionContext> _context;
      protected IMapper _mapper;
      protected Mock<HttpRequestData> _request;
      protected Mock<IEnvironmentRepository> _mockEnvironmentRepository;

      public FunctionUnitTestBase()
      {
         _mockEnvironmentRepository = new Mock<IEnvironmentRepository>();
         SetupFunctionContext();
         SetupAutoMapper();
         SetupHttpRequestResponse();
      }

      protected async Task<string> GetResponseBody(HttpResponseData response)
      {
         response.Body.Position = 0;
         StreamReader reader = new StreamReader(response.Body);
         return await reader.ReadToEndAsync();
      }

      protected void SetRequestBody(Mock<HttpRequestData> request, object requestObject)
      {
         byte[] byteArray = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(requestObject));
         MemoryStream bodyStream = new MemoryStream(byteArray);

         request.Setup(r => r.Body).Returns(bodyStream);
      }

      private void SetupFunctionContext()
      {
         ServiceCollection serviceCollection = new ServiceCollection();
         serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();
         ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

         _context = new Mock<FunctionContext>();
         _context.SetupProperty(c => c.InstanceServices, serviceProvider);
      }

      private void SetupAutoMapper()
      {
         MapperConfiguration config = new MapperConfiguration(cfg =>
         {
            cfg.AddProfile(new MappingProfile());
         });
         _mapper = config.CreateMapper();
      }

      private void SetupHttpRequestResponse()
      {
         _request = new Mock<HttpRequestData>(_context.Object);

         _request.Setup(r => r.CreateResponse()).Returns(() =>
         {
            Mock<HttpResponseData> response = new Mock<HttpResponseData>(_context.Object);
            response.SetupProperty(r => r.Headers, new HttpHeadersCollection());
            response.SetupProperty(r => r.StatusCode);
            response.SetupProperty(r => r.Body, new MemoryStream());
            return response.Object;
         });
      }
   }
}