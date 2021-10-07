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
         [TimerTrigger("%DatabaseBufferTimer%")] TimerInfo myTimer, // TODO: what schedule?
         FunctionContext context)
      {
         // TODO: get Logger<T> injected?
         ILogger logger = context.GetLogger(nameof(DatabaseBufferMonitor));

         logger.LogInformation($"{nameof(DatabaseBufferMonitor)} TimerTrigger Function executed at: {DateTime.Now}");
         if (myTimer.IsPastDue)
         {
            logger.LogInformation($"TimerInfo.IsPastDue");
         }

         try
         {
            await _dbBufferMonitorHelper.CheckHasDatabaseThreshold();
         }
         catch (Exception ex)
         {
            logger.LogInformation($"{ex.GetType()} : {ex.Message}");
         }
      }
   }
}
