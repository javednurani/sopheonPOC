using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Products.AspNetCore.HealthCheck
{
   public class EnvironmentDatabaseHealthCheck : IHealthCheck
   {
      private readonly IEnvironmentSqlConnectionStringProvider _environmentSqlConnectionStringProvider;

      public EnvironmentDatabaseHealthCheck(IEnvironmentSqlConnectionStringProvider environmentSqlConnectionStringProvider) 
      {
         _environmentSqlConnectionStringProvider = environmentSqlConnectionStringProvider;
      }

      public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
      {
         string connectionString = await _environmentSqlConnectionStringProvider.GetConnectionStringAsync();
         using (var connection = new SqlConnection(connectionString))
         {
            try
            {
               await connection.OpenAsync(cancellationToken);
            }
            catch (Exception)
            {
               return new HealthCheckResult(context.Registration.FailureStatus, "Dependency unhealthy; cannot reach environment database");
            }
            return HealthCheckResult.Healthy("Dependency responding as expected");
         }
      }
   }
}
