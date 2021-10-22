using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Environments.Functions.Helpers;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class DatabaseBufferMonitor
   {
      private const string INPUT_BINDING_BLOB_PATH = "armtemplates/ElasticPoolWithBuffer/ElasticPool_Database_Buffer.json";

      private readonly IConfiguration _configuration;
      private readonly IDatabaseBufferMonitorHelper _dbBufferMonitorHelper;

      public DatabaseBufferMonitor(IDatabaseBufferMonitorHelper dbBufferMonitorHelper, IConfiguration configuration)
      {
         _dbBufferMonitorHelper = dbBufferMonitorHelper;
         _configuration = configuration;
      }

      [Function(nameof(DatabaseBufferMonitor))]
      public async Task Run(
         [TimerTrigger("%DatabaseBufferTimer%")] TimerInfo myTimer,
         [BlobInput(INPUT_BINDING_BLOB_PATH)] string jsonTemplateData,
         FunctionContext context)
      {
         ILogger logger = context.GetLogger(nameof(DatabaseBufferMonitor));
         logger.LogInformation($"{nameof(DatabaseBufferMonitor)} TimerTrigger Function executed at: {DateTime.Now}");

         try
         {
            string subscriptionId = Environment.GetEnvironmentVariable("AzSubscriptionId");
            string resourceGroupName = Environment.GetEnvironmentVariable("AzResourceGroupName");
            string sqlServerName = Environment.GetEnvironmentVariable("AzSqlServerName");
            string adminLoginEnigma = _configuration["SqlServerAdminEnigma"]; // Pull admin enigma from app config (user secrets or key vault)

            if(string.IsNullOrEmpty(jsonTemplateData))
            {
               throw new NullReferenceException($"File was not found in blob storage at path: {INPUT_BINDING_BLOB_PATH}");
            }
            jsonTemplateData = jsonTemplateData
               .Replace(StringConstants.SERVER_NAME_TOKEN, sqlServerName)
               .Replace(StringConstants.ADMINISTRATOR_LOGIN_ENIGMA_TOKEN, adminLoginEnigma);

            await _dbBufferMonitorHelper.EnsureDatabaseBufferAsync(subscriptionId, resourceGroupName, sqlServerName, jsonTemplateData);
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
            throw;
         }
      }
   }
}
