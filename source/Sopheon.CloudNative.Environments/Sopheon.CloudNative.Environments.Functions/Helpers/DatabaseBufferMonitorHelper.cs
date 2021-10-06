using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
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
      private const int BUFFER_MIN_CAPACITY = 5;

      private readonly ILogger<DatabaseBufferMonitorHelper> _logger;
      private readonly IAzure _azure;

      public DatabaseBufferMonitorHelper(ILogger<DatabaseBufferMonitorHelper> logger, IAzure azure)
      {
         _logger = logger;
         _azure = azure;
      }

      public async Task<bool> CheckHasDatabaseThreshold()
      {
         _logger.LogInformation($"Authenticated with Service Principal to Subscription: {_azure.SubscriptionId}!");

         string subscriptionId = Environment.GetEnvironmentVariable("AzSubscriptionId");
         string resourceGroupName = Environment.GetEnvironmentVariable("AzResourceGroupName");
         string sqlServerName = Environment.GetEnvironmentVariable("AzSqlServerName");

         #region BufferCapacity
         ISqlServer sqlServer = await _azure.SqlServers
                  .GetByIdAsync($"/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Sql/servers/{sqlServerName}");

         List<ISqlDatabase> notAssigned = new List<ISqlDatabase>();
         List<ISqlDatabase> assignedToCustomer = new List<ISqlDatabase>(); // TODO: doo weneed this?

         IReadOnlyList<ISqlDatabase> allDatabasesOnServer = await _azure.SqlServers.Databases.ListBySqlServerAsync(sqlServer);

         // categorize CustomerProvisionedDatabase tagged databases by tag value
         foreach (var database in allDatabasesOnServer) // TODO: optimize search?
         {
            ISqlDatabase databaseWithDetails = await _azure.SqlServers.Databases.GetBySqlServerAsync(sqlServer, database.Name);

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

         if (notAssigned.Count() >= BUFFER_MIN_CAPACITY)
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

         // TODO: return on some deployment conditions - provisioningStatus / tags etc
         IPagedCollection<IDeployment> deploymentsForResourceGroup = await _azure.Deployments.ListByResourceGroupAsync(resourceGroupName);

         #endregion // ExistingDeployments

         #region CreateDeployment

         // TODO: calculate what to deploy?
         IReadOnlyList<ISqlElasticPool> elasticPools = await _azure.SqlServers.ElasticPools
            .ListBySqlServerAsync(resourceGroupName, sqlServerName);

         // TODO: create deployment
         // https://docs.microsoft.com/en-us/azure/azure-resource-manager/

         // https://github.com/Azure-Samples/resources-dotnet-deploy-using-arm-template
         _azure.Deployments
            .Define(null)
            .WithExistingResourceGroup("")
            .WithTemplateLink(null, null)
            .WithParameters(default(object))
            .WithMode(default(DeploymentMode))
            .Create();

         #endregion // CreateDeployment

         // TODO: write ENV.Resources records?

         // TODO: what to return?
         return true;
      }
   }
}
