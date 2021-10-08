using System;
using System.Net.Http;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure
{
   public class FunctionFactDependencyChecker : IDependencyChecker
   {
      private static readonly string _url = "http://localhost:7071/openapi/1.0";  // TODO: where should this url live?
      private static readonly Lazy<DependencyCheckResult> lazyResult = new(CheckDependencyInternal);

      private static DependencyCheckResult CheckDependencyInternal()
      {
         try
         {
            Console.WriteLine($"{nameof(FunctionFactDependencyChecker)} is checking dependencies...");

            _ = new HttpClient().GetAsync(_url).Result;
            return DependencyCheckResult.Success();
         }
         catch (Exception ex)
         {
            string failureDescription = $"Functions don't seem to be running at '{_url}'" +
               Environment.NewLine +
               ex.Message;
            return DependencyCheckResult.Failure(failureDescription);
         }
      }

      public DependencyCheckResult CheckDependency()
      {
         return lazyResult.Value;
      }
   }
}
