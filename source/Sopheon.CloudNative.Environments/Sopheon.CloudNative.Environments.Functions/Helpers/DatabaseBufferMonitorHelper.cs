using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
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
      private const int BUFFER_CAPACITY = 5;

      // TODO: how to get logger instance?  passed in from caller?  get new instance from context?
      ILogger logger;

      public async Task<bool> CheckHasDatabaseThreshold()
      {
         IAzure azure = await AuthenticateWithAzureServicePrincipal(logger);
         logger.LogInformation($"Authenticated with Service Principal to Subscription: {azure.SubscriptionId}!");

         string subscriptionId = Environment.GetEnvironmentVariable("AzSubscriptionId");
         string resourceGroupName = Environment.GetEnvironmentVariable("AzResourceGroupName");
         string sqlServerName = Environment.GetEnvironmentVariable("AzSqlServerName");

         #region BufferCapacity
         ISqlServer sqlServer = await azure.SqlServers
                  .GetByIdAsync($"/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Sql/servers/{sqlServerName}");

         List<ISqlDatabase> notAssigned = new List<ISqlDatabase>();
         List<ISqlDatabase> assignedToCustomer = new List<ISqlDatabase>();

         IReadOnlyList<ISqlDatabase> allDatabasesOnServer = await azure.SqlServers.Databases.ListBySqlServerAsync(sqlServer);

         // categorize CustomerProvisionedDatabase tagged databases by tag value
         foreach (var database in allDatabasesOnServer) // TODO: optimize search?
         {
            ISqlDatabase databaseWithDetails = await azure.SqlServers.Databases.GetBySqlServerAsync(sqlServer, database.Name);

            // has CustomerProvisionedDatabase tag
            string tagValue;
            if (databaseWithDetails.Tags != null && databaseWithDetails.Tags.TryGetValue(CUSTOMER_PROVISIONED_DATABASE_TAG_NAME, out tagValue))
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

         if (notAssigned.Count() >= BUFFER_CAPACITY)
         {
            // TODO: update message
            logger.LogInformation($"Sufficient database buffer capacity. Exiting {nameof(DatabaseBufferMonitor)}...");

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

         // TODO: return on some deployment conditions - provisioningStatus / tags etc
         IPagedCollection<IDeployment> deploymentsForResourceGroup = await azure.Deployments.ListByResourceGroupAsync(resourceGroupName);

         #endregion // ExistingDeployments

         #region CreateDeployment

         // TODO: calculate what to deploy?
         IReadOnlyList<ISqlElasticPool> elasticPools = await azure.SqlServers.ElasticPools
            .ListBySqlServerAsync(resourceGroupName, sqlServerName);

         // create deployment
         // https://docs.microsoft.com/en-us/azure/azure-resource-manager/

         #endregion // CreateDeployment

         // TODO: write ENV.Resources records?

         // TODO: what to return?
         return true;
      }

      private static async Task<IAzure> AuthenticateWithAzureServicePrincipal(ILogger logger)
      {
         // authenticate with Service Principal credentials
         logger.LogInformation("Fetching Service Principal credentials");
         string clientId = Environment.GetEnvironmentVariable("AzSpClientId");
         string clientSecret = Environment.GetEnvironmentVariable("AzSpClientSecret");
         string tenantId = Environment.GetEnvironmentVariable("AzSpTenantId");

         AzureCredentials credentials = SdkContext.AzureCredentialsFactory
            .FromServicePrincipal(clientId, clientSecret, tenantId, environment: AzureEnvironment.AzureGlobalCloud);

         logger.LogInformation($"Authenticating with Azure...");

         return await Microsoft.Azure.Management.Fluent.Azure
            .Authenticate(credentials)
            .WithDefaultSubscriptionAsync();
      }
   }
}
