using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Sopheon.CloudNative.Environments.Functions.Get
{
   public static class GetEnvironments
   {
      [Function(nameof(GetEnvironments))]
      public static async Task<HttpResponseData> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req,
          FunctionContext context)
      {
         var logger = context.GetLogger(nameof(GetEnvironments));
         logger.LogInformation("C# HTTP trigger function processed a request.");

         var queryDictionary = QueryHelpers.ParseQuery(req.Url.Query);
         string name;
         try
         {
            name = queryDictionary["name"];
         }
         catch
         {
            name = "DEFAULT";
         }

         string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
         dynamic data = JsonConvert.DeserializeObject(requestBody);
         name = name ?? data?.name;

         string responseMessage = string.IsNullOrEmpty(name)
             ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
             : $"Hello, {name}. This HTTP triggered function executed successfully.";

         HttpResponseData okResponse = req.CreateResponse(System.Net.HttpStatusCode.OK);
         await okResponse.WriteAsJsonAsync(responseMessage);

         return okResponse;
      }
   }
}
