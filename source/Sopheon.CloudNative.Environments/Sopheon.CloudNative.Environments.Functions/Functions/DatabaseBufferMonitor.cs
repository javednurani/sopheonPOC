using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.Extensions.Logging;

namespace Sopheon.CloudNative.Environments.Functions
{
   public static class DatabaseBufferMonitor
   {
      private const string NCRONTAB_EVERY_SECOND = "* * * * * *";
      private const string NCRONTAB_EVERY_5_SECONDS = "*/5 * * * * *";
      private const string NCRONTAB_EVERY_MINUTE = "0 * * * * *";

      [Function(nameof(DatabaseBufferMonitor))]
      public static async void Run([TimerTrigger(NCRONTAB_EVERY_MINUTE)] TimerInfo myTimer, FunctionContext context)
      {
         ILogger logger = context.GetLogger(nameof(DatabaseBufferMonitor));
         logger.LogInformation($"{nameof(DatabaseBufferMonitor)} Timer trigger function executed at: {DateTime.Now}");

         try
         {
            // TODO 9/30 https://github.com/Azure/azure-functions-dotnet-worker/issues/534

            IAzure azure = AuthenticateWithAzureServicePrincipal(logger);
            logger.LogInformation($"Authenticated!");

            // check buffer capacity
            // case 1 - single server, single elastic pool 
            var sqlServer = await azure.SqlServers.GetByIdAsync("...");

            // case 2 - single server, multiple elastic pools
            // case 3 - multiple servers, multiple elastic pool 

            IReadOnlyList<ISqlElasticPool> elasticPools = await azure.SqlServers.ElasticPools
               .ListBySqlServerAsync("...", "...");

            //var databases = await azure.SqlServers.Databases.ListBySqlServerAsync();

            // return if satisfied

            // calculate what to deploy?

            // create deployment

            // write ENV.Resources records?

         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
         }
      }

      private static IAzure AuthenticateWithAzureServicePrincipal(ILogger logger)
      {
         // authenticate with Service Principal credentials
         logger.LogInformation("Fetching Service Principal credentials");
         string clientId = Environment.GetEnvironmentVariable("AzSpClientId");
         string clientSecret = Environment.GetEnvironmentVariable("AzSpClientSecret");
         string tenantId = Environment.GetEnvironmentVariable("AzSpTenantId");

         AzureCredentials credentials = SdkContext.AzureCredentialsFactory
            .FromServicePrincipal(clientId, clientSecret, tenantId, environment: AzureEnvironment.AzureGlobalCloud);

         logger.LogInformation($"Authenticating with Azure...");

         return Microsoft.Azure.Management.Fluent.Azure
            .Authenticate(credentials)
            .WithSubscription("...");
      }

   }
}
