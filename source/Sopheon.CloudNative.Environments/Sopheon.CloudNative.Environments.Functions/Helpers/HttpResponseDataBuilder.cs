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
      public async Task<HttpResponseData> BuildWithJsonBody<T>(HttpRequestData request, HttpStatusCode statusCode, T body, ObjectSerializer serializer)
      {
         HttpResponseData createdResponse = request.CreateResponse();
         await createdResponse.WriteAsJsonAsync(body, serializer, statusCode);
         return createdResponse;
      }
      public async Task<HttpResponseData> BuildWithStringBody(HttpRequestData request, HttpStatusCode statusCode, string body)
      {
         HttpResponseData missingNameResponse = request.CreateResponse(statusCode);
         await missingNameResponse.WriteStringAsync(body);
         return missingNameResponse;
      }
   }
}
