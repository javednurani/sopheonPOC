using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Sopheon.CloudNative.Environments.Domain.Queries;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Functions.Validators;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Functions
{
   public class FunctionUnitTestBase
   {
      protected Mock<FunctionContext> _context;
      protected IMapper _mapper;
      protected Mock<HttpRequestData> _request;
      protected Mock<IEnvironmentRepository> _mockEnvironmentRepository;
      protected HttpResponseDataBuilder _responseBuilder;
      protected IValidator<EnvironmentDto> _environmentDtoValidator;
      protected Mock<IEnvironmentQueries> _mockEnvironmentQueries;
      protected Mock<IConfiguration> _mockConfiguration;

      public FunctionUnitTestBase()
      {
         _mockEnvironmentRepository = new Mock<IEnvironmentRepository>();
         _responseBuilder = new HttpResponseDataBuilder();
         _environmentDtoValidator = new EnvironmentDtoValidator();
         _mockEnvironmentQueries = new Mock<IEnvironmentQueries>();
         _mockConfiguration = new Mock<IConfiguration>();

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