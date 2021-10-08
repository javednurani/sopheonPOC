using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Environments.Functions.Helpers;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class DatabaseBufferMonitor
   {
      // TODO: consolidate to constants?
      public static string SERVER_NAME_TOKEN = "^SqlServerName^";
      public static string ADMINISTRATOR_LOGIN_ENIGMA_TOKEN = "^SqlAdminEngima^";

      private IDatabaseBufferMonitorHelper _dbBufferMonitorHelper;

      public DatabaseBufferMonitor(IDatabaseBufferMonitorHelper dbBufferMonitorHelper)
      {
         _dbBufferMonitorHelper = dbBufferMonitorHelper;
      }

      [Function(nameof(DatabaseBufferMonitor))]
      public async Task Run(
         [TimerTrigger("%DatabaseBufferTimer%")] TimerInfo myTimer,
         [BlobInput("TODO BLOB PATH", Connection = "TODO BLOB CONNECTION")] string jsonTemplateData,
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

            // TODO: get database password from key vault
            string adminLoginEnigma = string.Empty;

            // TODO: string replace SqlServerName and password into json template
            jsonTemplateData = jsonTemplateData
               .Replace(SERVER_NAME_TOKEN, sqlServerName)
               .Replace(ADMINISTRATOR_LOGIN_ENIGMA_TOKEN, adminLoginEnigma);

            _= await _dbBufferMonitorHelper.CheckHasDatabaseThreshold(subscriptionId, resourceGroupName, sqlServerName, jsonTemplateData);
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");

            // TODO: throw?  return something?  we could silently fail other than log statements
         }
      }
   }
}
