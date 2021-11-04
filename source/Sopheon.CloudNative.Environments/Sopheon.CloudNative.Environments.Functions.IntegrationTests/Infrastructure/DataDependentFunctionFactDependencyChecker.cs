using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Sopheon.CloudNative.Environments.Utility.TestData;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure
{
   public class DataDependentFunctionFactDependencyChecker : FunctionFactDependencyChecker, IDependencyChecker
   {
      private static readonly Lazy<DependencyCheckResult> lazyResult = new Lazy<DependencyCheckResult>(CheckDependencyInternal);

      private static DependencyCheckResult CheckDependencyInternal()
      {
         Console.WriteLine($"{nameof(DataDependentFunctionFactDependencyChecker)} is checking dependencies...");

         Environments_OpenApiClient environments_OpenApiClient = new Environments_OpenApiClient(new HttpClient());
         ICollection<EnvironmentDto> environments = environments_OpenApiClient.GetEnvironmentsAsync(TestDataConstants.OwnerKey1).Result;

         if (environments.Any(env => env.EnvironmentKey == TestDataConstants.EnvironmentKey1))
         {
            return DependencyCheckResult.Success();
         }
         else
         {
            string message = $"Functions are running at '{environments_OpenApiClient.BaseUrl}', but necessary test data has not been seeded.";
            return DependencyCheckResult.Failure(message);
         }
      }

      public new DependencyCheckResult CheckDependency()
      {
         // inherit skip reason from base Function class
         DependencyCheckResult baseResult = base.CheckDependency();

         if (baseResult.IsSuccessful)
         {
            return lazyResult.Value;
         }
         else
         {
            return baseResult;
         }
      }
   }
}
