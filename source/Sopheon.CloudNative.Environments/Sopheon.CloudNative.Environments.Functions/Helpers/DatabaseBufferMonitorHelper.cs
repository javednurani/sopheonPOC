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

      
      private readonly int bufferCapacity = int.Parse(Environment.GetEnvironmentVariable("DatabaseBufferCapacity"));
      private readonly ILogger<DatabaseBufferMonitorHelper> _logger;
      private readonly IAzure _azure;

      public DatabaseBufferMonitorHelper(ILogger<DatabaseBufferMonitorHelper> logger, IAzure azure)
      {
         _logger = logger;
         _azure = azure;
      }

      public async Task<bool> CheckHasDatabaseThreshold(string subscriptionId, string resourceGroupName, string sqlServerName, string deploymentTemplateJson)
      {
         _logger.LogInformation($"Authenticated with Service Principal to Subscription: {_azure.SubscriptionId}!");

         #region BufferCapacity
         ISqlServer sqlServer = await _azure.SqlServers
                  .GetByIdAsync($"/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Sql/servers/{sqlServerName}");

         List<ISqlDatabase> notAssigned = new List<ISqlDatabase>();
         List<ISqlDatabase> assignedToCustomer = new List<ISqlDatabase>(); // TODO: do we need this?

         IReadOnlyList<ISqlDatabase> allDatabasesOnServer = await _azure.SqlServers.Databases.ListBySqlServerAsync(sqlServer);

         // categorize CustomerProvisionedDatabase tagged databases by tag value
         foreach (var database in allDatabasesOnServer) // TODO: optimize search?
         {
            ISqlDatabase databaseWithDetails = await _azure.SqlServers.Databases.GetBySqlServerAsync(sqlServer, database.Name);

            // has CustomerProvisionedDatabase tag
            string tagValue = null;
            if (databaseWithDetails?.Tags?.TryGetValue(CUSTOMER_PROVISIONED_DATABASE_TAG_NAME, out tagValue) ?? false)
            {
               // CustomerProvisionedDatabase : NotAssigned
               if (tagValue == CUSTOMER_PROVISIONED_DATABASE_TAG_VALUE_INITIAL)
               {
                  notAssigned.Add(databaseWithDetails);
               }
               // CustomerProvisionedDatabase : AssignedToCustomer
               else if (tagValue == CUSTOMER_PROVISIONED_DATABASE_TAG_VALUE_ASSIGNED)
               {
                  assignedToCustomer.Add(databaseWithDetails);
               }
            }
         }

         if (notAssigned.Count() >= bufferCapacity)
         {
            // TODO: update message
            _logger.LogInformation($"Sufficient database buffer capacity. Exiting {nameof(DatabaseBufferMonitor)}...");

            // TODO: what to return
            return true;
         }
         #endregion // BufferCapacity

         #region BufferCapacityAlternate

         // track buffer capacity using ENV database tables ENV.Resources and ENV.EnvironmentResourcesBindings
         // add records to ENV.Resources table as DatabaseBufferMonitor creates Azure SQL Databases
         // search for buffer databases by finding ENV.Resources records that are of correct DomainResourceType and not referenced in ENV.EnvironmentResourceBindings
         // OR, track 'Assigned to Customer' status on ENV.Resources table

         #endregion // BufferCapacityAlternate

         #region ExistingDeployments
         IPagedCollection<IDeployment> deploymentsForResourceGroup = await _azure.Deployments.ListByResourceGroupAsync(resourceGroupName);

         if (deploymentsForResourceGroup.Any(d =>
            d.Name.Contains(nameof(DatabaseBufferMonitor)) &&
            _activeProvisioningStates.Contains(d.ProvisioningState)))
         {
            return true; // TODO, what to return
         }
         #endregion // ExistingDeployments

         #region CreateDeployment

         // TODO: calculate what to deploy?
         IReadOnlyList<ISqlElasticPool> elasticPools = await _azure.SqlServers.ElasticPools
            .ListBySqlServerAsync(resourceGroupName, sqlServerName);

         // TODO: create deployment
         // https://docs.microsoft.com/en-us/azure/azure-resource-manager/

         // https://github.com/Azure-Samples/resources-dotnet-deploy-using-arm-template
         string deploymentName = nameof(DatabaseBufferMonitor) + "_Deployment_" + DateTime.UtcNow.ToString("yyyyMMddTHHmmss");
         _logger.LogInformation($"Creating new deployment: {deploymentName}");
            
         IDeployment deployment = _azure.Deployments
            .Define(deploymentName)
            .WithExistingResourceGroup(resourceGroupName)
            .WithTemplate(deploymentTemplateJson)
            .WithParameters("{ }")
            .WithMode(DeploymentMode.Incremental)
            .Create();  // TODO: async?
            
         _logger.LogInformation($"Deployment: {deploymentName} was created successfully.");

         // TODO: need to check provision state?
         //deployment.ProvisioningState

         #endregion // CreateDeployment

         // TODO: write ENV.Resources records?

         // TODO: what to return?
         return true;
      }
   }
}
