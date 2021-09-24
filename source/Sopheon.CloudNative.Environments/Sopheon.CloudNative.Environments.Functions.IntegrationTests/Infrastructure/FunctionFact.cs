using System;
using System.Net.Http;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure
{
   /// <summary>
   /// The purpose of this class is to conditionally skip a function integration test at runtime
   /// if the function http endpoint is not responding.  This means the test could not be run,
   /// and the result is 'inconclusive'.
   /// This class will make one http request per total/overall test run.
   /// </summary>
   public class FunctionFact : FactAttribute
   {
      private static readonly HttpClient _httpClient = new HttpClient();
      protected static readonly string _url = "http://localhost:7071/openapi/1.0";  // TODO: where should this url live?
      protected static string _skipMessage = null;

      static FunctionFact()
      {
         // info: static so this will only execute once per total/overall test run

         try
         {
            _ = _httpClient.GetAsync(_url).Result;
         }
         catch (Exception ex)
         {
            _skipMessage = $"Functions dont seem to be running at '{_url}'" +
               Environment.NewLine +
               ex.Message;
         }
      }

      public FunctionFact()
      {
         // info: this will execute once per test class, but it's just pulling the static value
         this.Skip = _skipMessage;
      }
   }
}
