using System;
using Sopheon.CloudNative.Environments.Utility;

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
      public DataDependentFunctionFact()
      {
         DependencyCheckResult result = new DataDependentFunctionFactDependencyChecker().CheckDependency();
         if (!result.IsSuccessful)
         {
            this.Skip = result.FailureDescription;
         }
      }
   }
}
