using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.StandAlone
{
   public class DatabaseBufferMonitor_HappyPath : FunctionFact
   {
      [FunctionFact]
      public async Task HappyPath()
      {
         HttpResponseMessage response = await RunTimerTriggerFunction(nameof(DatabaseBufferMonitor));

         Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
      }

      private async Task<HttpResponseMessage> RunTimerTriggerFunction(string functionName)
      {
         string url = $"http://localhost:7071/admin/functions/{functionName}";

         using HttpClient client = new HttpClient();
         return await client.PostAsync(url,
            new StringContent(JsonSerializer.Serialize(new { input = "test" }), Encoding.UTF8, "application/json"));
      }
   }
}
