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

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Functions
{
   public class FunctionUnitTestBase
   {
      protected Mock<FunctionContext> _context;
      protected IMapper _mapper;

      public FunctionUnitTestBase()
      {
         SetupFunctionContext();
      }

      #region HTTP Helpers
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
      #endregion // HTTP Helpers

      #region Test Setup
      private void SetupFunctionContext()
      {
         ServiceCollection serviceCollection = new ServiceCollection();
         serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();
         ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

         _context = new Mock<FunctionContext>();
         _context.SetupProperty(c => c.InstanceServices, serviceProvider);
      }

      protected void SetupAutoMapper()
      {
         MapperConfiguration config = new MapperConfiguration(cfg =>
         {
            cfg.AddProfile(new MappingProfile());
         });
         _mapper = config.CreateMapper();
      }
      #endregion // Test Setup
   }
}