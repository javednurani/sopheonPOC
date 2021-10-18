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
      public FunctionFact()
      {
         DependencyCheckResult result = new FunctionFactDependencyChecker().CheckDependency();
         if(!result.IsSuccessful)
         {
            this.Skip = result.FailureDescription;
         }
      }
   }
}
