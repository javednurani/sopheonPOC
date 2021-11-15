using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Environments.Domain.Commands;
using Sopheon.CloudNative.Environments.Domain.Exceptions;

namespace Sopheon.CloudNative.Environments.Functions.Helpers
{
   public class AllocateSqlDatabaseSharedByServicesToEnvironmentHelper : IAllocateSqlDatabaseSharedByServicesToEnvironmentHelper
   {
      private readonly ILogger<AllocateSqlDatabaseSharedByServicesToEnvironmentHelper> _logger;
      private readonly HttpClient _httpClient;
      private readonly IAzure _azure;
      private readonly IEnvironmentCommands _environmentCommands;

      public AllocateSqlDatabaseSharedByServicesToEnvironmentHelper(
         ILogger<AllocateSqlDatabaseSharedByServicesToEnvironmentHelper> logger,
         IHttpClientFactory httpClientFactory,
         IAzure azure,
         IEnvironmentCommands environmentCommands)
      {
         _logger = logger;
         _httpClient = httpClientFactory.CreateClient(StringConstants.HTTP_CLIENT_NAME_AZURE_REST_API);
         _azure = azure;
         _environmentCommands = environmentCommands;
      }

      public async Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(Guid environmentKey, string subscriptionId, string resourceGroupName, string sqlServerName)
      {
         _logger.LogInformation($"Executing {nameof(AllocateSqlDatabaseSharedByServicesToEnvironmentAsync)}");

         ISqlDatabase sqlDatabase = await GetUnassignedSqlDatabaseAsync(subscriptionId, resourceGroupName, sqlServerName);

         // INFO: For ENV.Resources of type AzureSqlDb, ENV.Resources.Uri contains the Server & Database components of a SQL connection string
         string azureSqlDbResourceUri= $"Server=https://{sqlServerName}.database.windows.net;Database={sqlDatabase.Name};";
         await _environmentCommands.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(environmentKey, azureSqlDbResourceUri);

         await TagSqlDatabaseAsAssignedToCustomerAsync(sqlDatabase, subscriptionId, resourceGroupName, sqlServerName);
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

            if (databaseWithDetails?.Tags == null)
            {
               _logger.LogError($"Database details for '{database.Name}' were not found on Azure SQL Server: {sqlServer.Name}");
            }
            else if (databaseWithDetails.Tags.TryGetValue(StringConstants.CUSTOMER_PROVISIONED_DATABASE_TAG_NAME, out string tagValue)
               && tagValue == StringConstants.CUSTOMER_PROVISIONED_DATABASE_TAG_VALUE_INITIAL)
            {
               return databaseWithDetails;
            }
         }

         throw new CloudServiceException("No available database in buffer!");
      }

      private async Task TagSqlDatabaseAsAssignedToCustomerAsync(ISqlDatabase sqlDatabase, string subscriptionId, string resourceGroupName, string sqlServerName)
      {
         string url = $"https://management.azure.com/subscriptions/{subscriptionId}/resourcegroups/{resourceGroupName}/providers/Microsoft.Sql/servers/{sqlServerName}/databases/{sqlDatabase.Name}/providers/Microsoft.Resources/tags/default?api-version=2021-04-01";
         string body =
            "{" +
              "'operation': 'merge'," +
              "'properties': {" +
                "'tags': {" +
                  "'" + StringConstants.CUSTOMER_PROVISIONED_DATABASE_TAG_NAME + "': '" + StringConstants.CUSTOMER_PROVISIONED_DATABASE_TAG_VALUE_ASSIGNED + "'" +
                "}" +
              "}" +
            "}";

         HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, url)
         {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
         };

         HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage, CancellationToken.None);
         if (!response.IsSuccessStatusCode)
         {
            throw new CloudServiceException("Error calling Azure REST API to update SQL Database tag.");
         }

         return;
      }
   }
}
