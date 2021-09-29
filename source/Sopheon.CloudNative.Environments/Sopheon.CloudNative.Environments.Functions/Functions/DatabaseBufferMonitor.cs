using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Sopheon.CloudNative.Environments.Functions
{
   public static class DatabaseBufferMonitor
   {
      private const string NCRONTAB_EVERY_SECOND = "* * * * * *";
      private const string NCRONTAB_EVERY_5_SECONDS = "*/5 * * * * *";
      private const string NCRONTAB_EVERY_MINUTE = "0 * * * * *";

      [Function(nameof(DatabaseBufferMonitor))]
      public static void Run([TimerTrigger(NCRONTAB_EVERY_MINUTE)] TimerInfo myTimer, FunctionContext context)
      {
         ILogger logger = context.GetLogger(nameof(DatabaseBufferMonitor));
         logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
      }
   }
}
