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
      // TODO: consolidate to constants?
      public static string SERVER_NAME_TOKEN = "^SqlServerName^";
      public static string ADMINISTRATOR_LOGIN_ENIGMA_TOKEN = "^SqlAdminEngima^";

      private const string INPUT_BINDING_BLOB_PATH = "armtemplates/ElasticPoolWithBuffer/ElasticPool_Database_Buffer.json";

      private IDatabaseBufferMonitorHelper _dbBufferMonitorHelper;
      private readonly IConfiguration _configuration;
      
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

         // TODO: value in logging this?
         //if (myTimer.IsPastDue)
         //{
         //   logger.LogInformation($"TimerInfo.IsPastDue");
         //}

         try
         {
            string subscriptionId = Environment.GetEnvironmentVariable("AzSubscriptionId");
            string resourceGroupName = Environment.GetEnvironmentVariable("AzResourceGroupName");
            string sqlServerName = Environment.GetEnvironmentVariable("AzSqlServerName");

            // Pull admin enigma from app config (user secrets or key vault)
            string adminLoginEnigma = _configuration["SqlServerAdminEnigma"];

            //TODO check for null on jsonTemplateData in case the template is not found.
            jsonTemplateData = jsonTemplateData
               .Replace(SERVER_NAME_TOKEN, sqlServerName)
               .Replace(ADMINISTRATOR_LOGIN_ENIGMA_TOKEN, adminLoginEnigma);

            await _dbBufferMonitorHelper.EnsureDatabaseBufferAsync(subscriptionId, resourceGroupName, sqlServerName, jsonTemplateData);
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");

            // TODO: throw?  return something?  we could silently fail other than log statements
         }
      }
   }
}
