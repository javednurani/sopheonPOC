using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.ResourceManager.Fluent.Models;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.Extensions.Logging;

namespace Sopheon.CloudNative.Environments.Functions.Helpers
{
   public class DatabaseBufferMonitorHelper : IDatabaseBufferMonitorHelper
   {
      private readonly ProvisioningState[] _activeProvisioningStates = new ProvisioningState[]
      {
            ProvisioningState.Accepted,
            ProvisioningState.Running
      };

      private readonly ILogger<DatabaseBufferMonitorHelper> _logger;
      private readonly IAzure _azure;

      public DatabaseBufferMonitorHelper(ILogger<DatabaseBufferMonitorHelper> logger, IAzure azure)
      {
         _logger = logger;
         _azure = azure;
      }

      public async Task EnsureDatabaseBufferAsync(string subscriptionId, string resourceGroupName, string sqlServerName, string deploymentTemplateJson)
      {
         bool hasSufficientDatabaseBuffer = await HasSufficientDatabaseBuffer(subscriptionId, resourceGroupName, sqlServerName);
         if (hasSufficientDatabaseBuffer)
         {
            _logger.LogInformation($"Sufficient database buffer capacity. Exiting {nameof(DatabaseBufferMonitor)}...");
            return;
         }

         bool ongoingDeployment = await IsOngoingDeployment(resourceGroupName);
         if (ongoingDeployment)
         {
            _logger.LogInformation($"Ongoing database deployment. Exiting {nameof(DatabaseBufferMonitor)}...");
            return;
         }

         await PerformDeployment(resourceGroupName, deploymentTemplateJson);
      }

      private async Task<int> CheckBufferCount(ISqlServer sqlServer)
      {
         List<ISqlDatabase> notAssigned = new();

         IReadOnlyList<ISqlDatabase> allDatabasesOnServer = await _azure.SqlServers.Databases.ListBySqlServerAsync(sqlServer);

         // categorize CustomerProvisionedDatabase tagged databases by tag value
         foreach (var database in allDatabasesOnServer)
         {
            ISqlDatabase databaseWithDetails = await _azure.SqlServers.Databases.GetBySqlServerAsync(sqlServer, database.Name);

            if (databaseWithDetails?.Tags == null)
            {
               _logger.LogError($"Database details for '{database.Name}' were not found on Azure SQL Server: {sqlServer.Name}");
            }
            // has CustomerProvisionedDatabase tag
            if (databaseWithDetails.Tags.TryGetValue(StringConstants.CUSTOMER_PROVISIONED_DATABASE_TAG_NAME, out string tagValue)
               && tagValue == StringConstants.CUSTOMER_PROVISIONED_DATABASE_TAG_VALUE_INITIAL)
            {
               notAssigned.Add(databaseWithDetails);
            }
         }

         return notAssigned.Count;
      }

      private async Task<bool> HasSufficientDatabaseBuffer(string subscriptionId, string resourceGroupName, string sqlServerName)
      {
         ISqlServer sqlServer = await _azure.SqlServers
                           .GetByIdAsync($"/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Sql/servers/{sqlServerName}");

         bool validCapacity = int.TryParse(Environment.GetEnvironmentVariable("DatabaseBufferCapacity"), out int databaseBufferCapacity);
         if (!validCapacity)
         {
            throw new ArgumentException("DatabaseBufferCapacity is misconfigured, could not parse to an integer");
         }

         int bufferCount = await CheckBufferCount(sqlServer);
         return bufferCount >= databaseBufferCapacity;
      }

      private async Task<bool> IsOngoingDeployment(string resourceGroupName)
      {
         IPagedCollection<IDeployment> deploymentsForResourceGroup = await _azure.Deployments.ListByResourceGroupAsync(resourceGroupName);

         return deploymentsForResourceGroup.Any(d =>
            d.Name.Contains(nameof(DatabaseBufferMonitor)) &&
            _activeProvisioningStates.Contains(d.ProvisioningState));
      }

      private async Task PerformDeployment(string resourceGroupName, string deploymentTemplateJson)
      {
         string deploymentName = $"{nameof(DatabaseBufferMonitor)}_Deployment_{DateTime.UtcNow:yyyyMMddTHHmmss}";
         _logger.LogInformation($"Creating new deployment: {deploymentName}");

         IDeployment deployment = await _azure.Deployments
            .Define(deploymentName)
            .WithExistingResourceGroup(resourceGroupName)
            .WithTemplate(deploymentTemplateJson)
            .WithParameters("{ }")
            .WithMode(DeploymentMode.Incremental)
            .BeginCreateAsync();

         _logger.LogInformation($"Deployment: {deploymentName} was created successfully.");
      }
   }
}
