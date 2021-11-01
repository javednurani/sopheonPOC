using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Environments.Domain.Commands;

namespace Sopheon.CloudNative.Environments.Functions.Helpers
{
   public class ResourceAllocationHelper : IResourceAllocationHelper
   {
      private readonly ILogger<ResourceAllocationHelper> _logger;
      private readonly IAzure _azure;
      private readonly IEnvironmentCommands _environmentCommands;

      public ResourceAllocationHelper(
         ILogger<ResourceAllocationHelper> logger,
         IAzure azure,
         IEnvironmentCommands environmentCommands)
      {
         _logger = logger;
         _azure = azure;
         _environmentCommands = environmentCommands;
      }

      // TODO: params make sens to be coming from function right?
      public async Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(Guid environmentKey, string subscriptionId, string resourceGroupName, string sqlServerName)
      {
         _logger.LogInformation($"Executing {nameof(AllocateSqlDatabaseSharedByServicesToEnvironmentAsync)}");

         string resourceUri = await GetUnassignedSqlDatabaseAsync(subscriptionId, resourceGroupName, sqlServerName);

         // call EFCommands to assign SQLDatabase to environment
         await _environmentCommands.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(environmentKey, resourceUri);

         await TagSqlDatabaseAsAssignedToCustomer();
      }

      private async Task<string> GetUnassignedSqlDatabaseAsync(string subscriptionId, string resourceGroupName, string sqlServerName)
      {
         ISqlServer sqlServer = await _azure.SqlServers
                    .GetByIdAsync($"/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Sql/servers/{sqlServerName}");

         IReadOnlyList<ISqlDatabase> allDatabasesOnServer = await _azure.SqlServers.Databases.ListBySqlServerAsync(sqlServer);

         // find first unassigned database
         foreach (var database in allDatabasesOnServer)
         {
            ISqlDatabase databaseWithDetails = await _azure.SqlServers.Databases.GetBySqlServerAsync(sqlServer, database.Name);

            if (databaseWithDetails?.Tags?.TryGetValue(StringConstants.CUSTOMER_PROVISIONED_DATABASE_TAG_NAME, out string tagValue) ?? false
               && tagValue == StringConstants.CUSTOMER_PROVISIONED_DATABASE_TAG_VALUE_INITIAL)
            {
               // TODO: what info to return as "uri"?
               return databaseWithDetails.Name;
            }
         }

         // TODO: what does this case mean?  all DBs are assigned on server, error?
         throw new Exception("TODO");
      }

      private Task TagSqlDatabaseAsAssignedToCustomer()
      {
         // TODO: update SQL database tag to "AssignedToCustomer" (sp?) using Azure resource API
         return Task.CompletedTask;
      }
   }
}
