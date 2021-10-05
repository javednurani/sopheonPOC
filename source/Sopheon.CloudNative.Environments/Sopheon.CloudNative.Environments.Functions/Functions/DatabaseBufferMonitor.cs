using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Sql.Fluent;
using Microsoft.Extensions.Logging;

namespace Sopheon.CloudNative.Environments.Functions
{
   public static class DatabaseBufferMonitor
   {
      // timer schedules
      private const string NCRONTAB_EVERY_10_SECONDS = "*/10 * * * * *";
      private const string NCRONTAB_EVERY_MINUTE = "0 * * * * *";
      private const string NCRONTAB_EVERY_5_MINUTES = "0 */5 * * * *";
      private const string NCRONTAB_EVERY_DAY = "0 0 0 * * *";
      // known issue https://github.com/Azure/azure-functions-dotnet-worker/issues/534

      [Function(nameof(DatabaseBufferMonitor))]
      public static async Task Run([TimerTrigger(NCRONTAB_EVERY_DAY)] TimerInfo myTimer, FunctionContext context)
      {
         ILogger logger = context.GetLogger(nameof(DatabaseBufferMonitor));

         logger.LogInformation($"{nameof(DatabaseBufferMonitor)} TimerTrigger Function executed at: {DateTime.Now}");
         if (myTimer.IsPastDue)
         {
            logger.LogInformation($"TimerInfo.IsPastDue");
         }

         try
         {
            // TODO: call IDatabaseBufferMonitorHelper
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
         }
      }
   }
}
