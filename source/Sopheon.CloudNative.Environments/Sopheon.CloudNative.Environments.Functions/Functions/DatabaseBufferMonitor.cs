using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Environments.Functions.Helpers;

namespace Sopheon.CloudNative.Environments.Functions
{
   public class DatabaseBufferMonitor
   {
      private IDatabaseBufferMonitorHelper _dbBufferMonitorHelper;

      public DatabaseBufferMonitor(IDatabaseBufferMonitorHelper dbBufferMonitorHelper)
      {
         _dbBufferMonitorHelper = dbBufferMonitorHelper;
      }

      [Function(nameof(DatabaseBufferMonitor))]
      public async Task Run(
         [TimerTrigger("%DatabaseBufferTimer%")] TimerInfo myTimer,
         [Blob("TODO", FileAccess.Read, Connection = "TODO")] string jsonTemplateData,
         FunctionContext context)
      {
         ILogger logger = context.GetLogger(nameof(DatabaseBufferMonitor));

         logger.LogInformation($"{nameof(DatabaseBufferMonitor)} TimerTrigger Function executed at: {DateTime.Now}");

         // TODO: value in logging this?
         if (myTimer.IsPastDue)
         {
            logger.LogInformation($"TimerInfo.IsPastDue");
         }

         try
         {
            string subscriptionId = Environment.GetEnvironmentVariable("AzSubscriptionId");
            string resourceGroupName = Environment.GetEnvironmentVariable("AzResourceGroupName");
            string sqlServerName = Environment.GetEnvironmentVariable("AzSqlServerName");

            // TODO: get database password from key vault

            // TODO: string replace SqlServerName and password into json template

            await _dbBufferMonitorHelper.CheckHasDatabaseThreshold(subscriptionId, resourceGroupName, sqlServerName, jsonTemplateData);
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
         }
      }
   }
}
