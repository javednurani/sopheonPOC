﻿using Sopheon.CloudNative.Environments.Utility;
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
      private static Guid _environmentKey = TestData.EnvironmentKey1;
      private static string _dataDepSkipReason = null;   // need to have our own value, otherwise we will set for all FunctionFacts!

      static DataDependentFunctionFact()
      {
         // info: static so this will only execute once per total/overall test run

         // inherit skip reason from base Function class
         _dataDepSkipReason = _functionSkipReason;

         if (_dataDepSkipReason == null)
         {
            // no reason to skip test yet!

            ICollection<EnvironmentDto> environments = new Environments_OpenApiClient(new HttpClient()).GetEnvironmentsAsync().Result;

            if (!environments.Any(env => env.EnvironmentKey == _environmentKey))
            {
               _dataDepSkipReason = $"Functions are running at '{_url}', but necessary test data has not been seeded.";
            }
         }
      }

      public DataDependentFunctionFact()
      {
         // info: this will execute once per test class, but it's just pulling the (local) static value
         this.Skip = _dataDepSkipReason;
      }
   }
}
