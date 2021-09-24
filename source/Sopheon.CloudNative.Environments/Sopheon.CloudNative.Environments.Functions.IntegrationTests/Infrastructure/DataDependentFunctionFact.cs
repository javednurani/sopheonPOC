using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure

{
   /// <summary>
   /// The purpose of this class is to conditionally skip a function integration test which
   /// is data dependent at runtime if the environment does not contain the necessary test
   /// data.  This means the test could not be run, and the result is 'inconclusive'.
   /// This class will make one http request per total/overall test run.
   /// </summary>
   public class DataDependentFunctionFact : FunctionFact
   {
      // TODO: consolidate this key
      private static Guid _environmentKey = new Guid("EBA2CCBB-89D3-45E3-BF90-2DB160BF1552");

      static DataDependentFunctionFact()
      {
         // info: static so this will only execute once per total/overall test run

         if (_skipMessage == null)
         {
            // no reason to skip test yet!

            ICollection<EnvironmentDto> environments = new Environments_OpenApiClient(new HttpClient()).GetEnvironmentsAsync().Result;

            if (!environments.Any(env => env.EnvironmentKey == _environmentKey))
            {
               _skipMessage = $"Functions are running at '{_url}', but necessary test data has not been seeded.";
            }
         }
      }
   }
}
