using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Moq;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class FunctionUnitTestBase
   {

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
   }
}