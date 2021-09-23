using System;
using System.Net.Http;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests
{
   /// <summary>
   /// The purpose of this class is to conditionally skip a function integration test at runtime
   /// if the function http endpoint is not responding.  This means the test could not be run,
   /// and the result is 'inconclusive'
   /// </summary>
   public class FunctionFact : FactAttribute
   {
      // TODO: where should this url live?
      private static readonly string _url = "http://localhost:7071/openapi/1.0";

      public FunctionFact()
      {
         try
         {
            // TODO: could this be optimized so that only one test actually makes this http request?
            _ = new HttpClient().GetAsync(_url).Result;
         }
         catch (Exception ex)
         {
            this.Skip = $"Functions dont seem to be running at '{"http://localhost:7071/openapi/1.0"}'" +
               Environment.NewLine +
               ex.Message;
         }
      }
   }
}
