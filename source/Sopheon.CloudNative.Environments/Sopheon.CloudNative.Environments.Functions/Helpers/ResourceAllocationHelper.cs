using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
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
      private readonly HttpClient _httpClient;
      private readonly IAzure _azure;
      private readonly IEnvironmentCommands _environmentCommands;

      public ResourceAllocationHelper(
         ILogger<ResourceAllocationHelper> logger,
         IHttpClientFactory httpClientFactory,
         IAzure azure,
         IEnvironmentCommands environmentCommands)
      {
         _logger = logger;
         _httpClient = httpClientFactory.CreateClient("TODO"); // TODO: name?
         _azure = azure;
         _environmentCommands = environmentCommands;
      }

      // TODO: params make sens to be coming from function right?
      public async Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(Guid environmentKey, string subscriptionId, string resourceGroupName, string sqlServerName)
      {
         _logger.LogInformation($"Executing {nameof(AllocateSqlDatabaseSharedByServicesToEnvironmentAsync)}");

         ISqlDatabase sqlDatabase = await GetUnassignedSqlDatabaseAsync(subscriptionId, resourceGroupName, sqlServerName);

         // TODO: what info is "uri"?
         await _environmentCommands.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(environmentKey, sqlDatabase.Name);

         await TagSqlDatabaseAsAssignedToCustomer(sqlDatabase, subscriptionId, resourceGroupName);
      }

      private async Task<ISqlDatabase> GetUnassignedSqlDatabaseAsync(string subscriptionId, string resourceGroupName, string sqlServerName)
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
               return databaseWithDetails;
            }
         }

         // TODO: what does this case mean?  all DBs are assigned on server, error?
         throw new Exception("TODO");
      }

      private async Task TagSqlDatabaseAsAssignedToCustomer(ISqlDatabase sqlDatabase, string subscriptionId, string resourceGroupName)
      {
         string resourceProviderNamespace = "Microsoft.Sql";
         string parentResourcePath = "servers";
         string resourceType = "databases";
         string url = $"https://management.azure.com/subscriptions/{subscriptionId}/resourcegroups/{resourceGroupName}/providers/{resourceProviderNamespace}/{parentResourcePath}/{resourceType}/{sqlDatabase.Name}?api-version=2021-04-01";

         // TODO: use IHttpClientFactory?
         HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, url)
         {
            Content = new StringContent("TODO")
         };
         HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage, CancellationToken.None);
         if(!response.IsSuccessStatusCode)
         {
            // TODO: throw CloudServiceException?
            throw new Exception();
         }

         return;
      }
   }
}
