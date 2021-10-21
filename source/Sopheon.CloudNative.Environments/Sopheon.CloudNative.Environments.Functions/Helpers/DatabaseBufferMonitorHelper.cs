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
      // buffered database indicator using Azure Tags
      // TODO Cloud-1474: when we create a buffered DB, it should be tagged as CustomerProvisionedDatabase = NotAssigned
      private const string CUSTOMER_PROVISIONED_DATABASE_TAG_NAME = "CustomerProvisionedDatabase"; // Tag Name/key for Azure Resouce (Azure SQL database)
      private const string CUSTOMER_PROVISIONED_DATABASE_TAG_VALUE_INITIAL = "NotAssigned"; // databases with this Tag Value are part of buffer
      private const string CUSTOMER_PROVISIONED_DATABASE_TAG_VALUE_ASSIGNED = "AssignedToCustomer"; // not part of buffer

      // TODO - identify ProvisioningState lifecycle, confirm this list
      private readonly ProvisioningState[] _activeProvisioningStates = new ProvisioningState[]
      {
            ProvisioningState.Accepted,
            ProvisioningState.Created,
            ProvisioningState.Creating,
            ProvisioningState.Ready,
            ProvisioningState.Running,
            ProvisioningState.Updating
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

         bool ongoingDeployement = await IsOngoingDeployment(resourceGroupName);
         if (ongoingDeployement)
         {
            _logger.LogInformation($"Ongoing database deployment. Exiting {nameof(DatabaseBufferMonitor)}...");
            return;
         }

         await PerformDeployment(resourceGroupName, deploymentTemplateJson);
      }

      private async Task<int> CheckBufferCount(ISqlServer sqlServer)
      {
         List<ISqlDatabase> notAssigned = new List<ISqlDatabase>();

         IReadOnlyList<ISqlDatabase> allDatabasesOnServer = await _azure.SqlServers.Databases.ListBySqlServerAsync(sqlServer);

         // categorize CustomerProvisionedDatabase tagged databases by tag value
         foreach (var database in allDatabasesOnServer) // TODO: optimize search?
         {
            ISqlDatabase databaseWithDetails = await _azure.SqlServers.Databases.GetBySqlServerAsync(sqlServer, database.Name);

            // has CustomerProvisionedDatabase tag
            if (databaseWithDetails?.Tags?.TryGetValue(CUSTOMER_PROVISIONED_DATABASE_TAG_NAME, out string tagValue) ?? false
               && tagValue == CUSTOMER_PROVISIONED_DATABASE_TAG_VALUE_INITIAL)
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
         string deploymentName = $"{nameof(DatabaseBufferMonitor)}_Deployment_{DateTime.UtcNow.ToString("yyyyMMddTHHmmss")}";
         _logger.LogInformation($"Creating new deployment: {deploymentName}");

         IDeployment deployment = await _azure.Deployments
            .Define(deploymentName)
            .WithExistingResourceGroup(resourceGroupName)
            .WithTemplate(deploymentTemplateJson)
            .WithParameters("{ }")
            .WithMode(DeploymentMode.Incremental)
            .CreateAsync();

         _logger.LogInformation($"Deployment: {deploymentName} was created successfully.");
      }
   }
}
