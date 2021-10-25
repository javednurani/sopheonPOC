using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.StandAlone
{
   public class DatabaseBufferMonitor_HappyPath : FunctionFact
   {
      [FunctionFact]
      public async Task HappyPath()
      {
         HttpResponseMessage response = await ExecuteTimerTriggerFunction(nameof(DatabaseBufferMonitor));

         Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
      }

      private static async Task<HttpResponseMessage> ExecuteTimerTriggerFunction(string functionName)
      {
         string url = $"http://localhost:7071/admin/functions/{functionName}";
         string json = JsonSerializer.Serialize(new { });
         StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

         return await new HttpClient().PostAsync(url, content);
      }
   }
}
