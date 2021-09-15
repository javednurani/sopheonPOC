using Azure.Core.Serialization;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Environments.Functions.Helpers
{
   public class HttpResponseDataBuilder
   {
      public async Task<HttpResponseData> BuildWithJsonBody<T>(HttpRequestData request, HttpStatusCode statusCode, T body)
      {
         HttpResponseData createdResponse = request.CreateResponse();

         // Cloud-1487, need to provide instance of Azure.Core.Serialization.ObjectSerializer
         // host config in Program.cs main() doesn't run in unit test context, so doesn't provide an ObjectSerializer
         await createdResponse.WriteAsJsonAsync(body, new JsonObjectSerializer(), statusCode);
         return createdResponse;
      }

      public async Task<HttpResponseData> BuildWithStringBody(HttpRequestData request, HttpStatusCode statusCode, string body)
      {
         HttpResponseData missingNameResponse = request.CreateResponse(statusCode);
         await missingNameResponse.WriteStringAsync(body);
         return missingNameResponse;
      }

      public HttpResponseData BuildWithoutBody(HttpRequestData request, HttpStatusCode statusCode)
      {
         HttpResponseData missingNameResponse = request.CreateResponse(statusCode);
         return missingNameResponse;
      }
   }
}
