using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Products.AspNetCore.HealthCheck
{
   public class EnvironmentCatalogHealthCheck : IHealthCheck
   {
      public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
      {
         bool healthCheckResultHealthy = true; // TODO: Ping Environment Lookup Service

         if (healthCheckResultHealthy)
         {
            return Task.FromResult(HealthCheckResult.Healthy("Dependency responding as expected"));
         }

         return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, "Dependency unhealthy; cannot reach environment catalog"));
      }
   }
}
