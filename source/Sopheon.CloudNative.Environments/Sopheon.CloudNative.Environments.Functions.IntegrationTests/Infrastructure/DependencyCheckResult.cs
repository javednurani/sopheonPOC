namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure
{
   public class DependencyCheckResult
   {
      private DependencyCheckResult()
      {
      }

      private DependencyCheckResult(bool isSuccessful, string failureDescription)
      {
         IsSuccessful = isSuccessful;
         FailureDescription = failureDescription;
      }

      public static DependencyCheckResult Success()
      {
         return new DependencyCheckResult(true, null);
      }

      public static DependencyCheckResult Failure(string failureDescription)
      {
         return new DependencyCheckResult(false, failureDescription);
      }

      public bool IsSuccessful { get; init; }

      public string FailureDescription { get; init; }
   }
}
