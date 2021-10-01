using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.Extensions.Logging;

namespace Sopheon.CloudNative.Environments.Functions
{
   public static class DatabaseBufferMonitor
   {
      private const int BUFFER_CAPACITY = 5;

      // timer schedules
      private const string NCRONTAB_EVERY_10_SECONDS = "*/10 * * * * *";
      private const string NCRONTAB_EVERY_MINUTE = "0 * * * * *";
      private const string NCRONTAB_EVERY_5_MINUTES = "0 */5 * * * *";
      private const string NCRONTAB_EVERY_DAY = "0 0 0 * * *";
      // known issue https://github.com/Azure/azure-functions-dotnet-worker/issues/534

      // buffered database indicator using Azure Tags
      // TODO Cloud-1474: when we create a buffered DB, it should be tagged as CustomerProvisionedDatabase = NotAssigned
      private const string CUSTOMER_PROVISIONED_DATABASE_TAG_NAME = "CustomerProvisionedDatabase"; // Tag Name/key for Azure Resouce (Azure SQL database)
      private const string CUSTOMER_PROVISIONED_DATABASE_TAG_VALUE_INITIAL = "NotAssigned"; // databases with this Tag Value are part of buffer
      private const string CUSTOMER_PROVISIONED_DATABASE_TAG_VALUE_ASSIGNED = "AssignedToCustomer"; // not part of buffer

      [Function(nameof(DatabaseBufferMonitor))]
      public static async Task Run([TimerTrigger(NCRONTAB_EVERY_DAY)] TimerInfo myTimer, FunctionContext context)
      {
         ILogger logger = context.GetLogger(nameof(DatabaseBufferMonitor));

         logger.LogInformation($"{nameof(DatabaseBufferMonitor)} TimerTrigger Function executed at: {DateTime.Now}");
         if (myTimer.IsPastDue)
         {
            logger.LogInformation($"TimerInfo.IsPastDue");
         }

         try
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
               logger.LogInformation($"Sufficient database buffer capacity. Exiting {nameof(DatabaseBufferMonitor)}...");
               return;
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
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
         }
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
