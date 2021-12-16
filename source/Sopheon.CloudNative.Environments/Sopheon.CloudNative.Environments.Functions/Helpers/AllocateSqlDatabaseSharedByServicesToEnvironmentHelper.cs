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
using Sopheon.CloudNative.Environments.Domain.Models;
using Sopheon.CloudNative.Environments.Domain.Queries;

namespace Sopheon.CloudNative.Environments.Functions.Helpers
{
   public class AllocateSqlDatabaseSharedByServicesToEnvironmentHelper : IAllocateSqlDatabaseSharedByServicesToEnvironmentHelper
   {
      private readonly ILogger<AllocateSqlDatabaseSharedByServicesToEnvironmentHelper> _logger;
      private readonly HttpClient _httpClient;
      private readonly IEnvironmentCommands _environmentCommands;
      private readonly IEnvironmentQueries _environmentQueries;

      public AllocateSqlDatabaseSharedByServicesToEnvironmentHelper(
         ILogger<AllocateSqlDatabaseSharedByServicesToEnvironmentHelper> logger,
         IHttpClientFactory httpClientFactory,
         IEnvironmentQueries environmentQueries,
         IEnvironmentCommands environmentCommands
      )
      {
         _logger = logger;
         _httpClient = httpClientFactory.CreateClient(StringConstants.HTTP_CLIENT_NAME_AZURE_REST_API);
         _environmentCommands = environmentCommands;
         _environmentQueries = environmentQueries;
      }

      public async Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(Guid environmentKey, string subscriptionId, string resourceGroupName, string sqlServerName)
      {
         _logger.LogInformation($"Executing {nameof(AllocateSqlDatabaseSharedByServicesToEnvironmentAsync)}");

         Resource resource = await _environmentQueries.GetUnassignedResource(Domain.Enums.ResourceTypes.AzureSqlDb);

         // INFO: For ENV.Resources of type AzureSqlDb, ENV.Resources.Uri contains the Server & Database components of a SQL connection string
         await _environmentCommands.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(environmentKey, resource);

         await TagSqlDatabaseAsAssignedToCustomerAsync(resource.Name, subscriptionId, resourceGroupName, sqlServerName);
      }

      private async Task TagSqlDatabaseAsAssignedToCustomerAsync(string sqlDatabase, string subscriptionId, string resourceGroupName, string sqlServerName)
      {
         string url = $"https://management.azure.com/subscriptions/{subscriptionId}/resourcegroups/{resourceGroupName}/providers/Microsoft.Sql/servers/{sqlServerName}/databases/{sqlDatabase}/providers/Microsoft.Resources/tags/default?api-version=2021-04-01";
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
            string logMessage = "Error calling Azure REST API to update SQL Database tag." + System.Environment.NewLine +
               $"Status Code: {response.StatusCode}" + System.Environment.NewLine +
               $"Reason: {response.ReasonPhrase}";
            _logger.LogError(logMessage);

            throw new CloudServiceException("Error calling Azure REST API to update SQL Database tag.");
         }

         return;
      }
   }
}
