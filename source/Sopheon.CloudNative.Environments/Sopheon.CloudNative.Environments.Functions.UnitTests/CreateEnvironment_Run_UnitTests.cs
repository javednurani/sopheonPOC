using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Sopheon.CloudNative.Environments.Functions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class CreateEnvironment_Run_UnitTests
   {
      public CreateEnvironment_Run_UnitTests()
      {
         TestSetup();
      }
      [Fact]
      public async void Run_RequestBodyMissingName_ReturnsBadRequest()
      {
         // Arrange

         // request DTO

         EnvironmentDto environmentDto = new EnvironmentDto
         {
            //Name = "asdf",
            Owner = Guid.NewGuid(),
            Description = "asdf"
         };

         // FunctionContext

         var serviceCollection = new ServiceCollection();
         serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();
         var serviceProvider = serviceCollection.BuildServiceProvider();

         var context = new Mock<FunctionContext>();
         context.SetupProperty(c => c.InstanceServices, serviceProvider);

         // HttpRequestData

         var byteArray3 = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(environmentDto));
         var bodyStream3 = new MemoryStream(byteArray3);


         var request = new Mock<HttpRequestData>(context.Object);
         request.CallBase = true;
         request.Setup(r => r.Body).Returns(bodyStream3);

         //request.Setup(r => r.CreateResponse()).Returns(() =>
         //{
         //   var response = new Mock<HttpResponseData>(context.Object);
         //   response.CallBase = true;

         //   response.SetupProperty(r => r.Headers, new HttpHeadersCollection());
         //   response.SetupProperty(r => r.StatusCode);
         //   response.SetupProperty(r => r.Body, new MemoryStream());
         //   //response.Setup(r => r.WriteStringAsync(It.IsAny<string>(), null)).Returns(() => Task.FromResult(typeof(void)));
         //   //response.Setup(r => r.WriteStringAsync(It.IsAny<string>(), null)).Callback<string>(s => response.Object.Body.Write(Encoding.ASCII.GetBytes(s)));
         //   return response.Object;
         //});


         // Act
         CreateEnvironment sut = new CreateEnvironment(null, null);

         HttpResponseData result = await sut.Run(request.Object, context.Object);

         // Assert
         Assert.NotNull(result);
         Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

         string responseBody = await new StreamReader(result.Body).ReadToEndAsync();

         Assert.Equal("asdf1243", responseBody);

      }

      private void TestSetup()
      {
         // TODO
      }
   }
}
