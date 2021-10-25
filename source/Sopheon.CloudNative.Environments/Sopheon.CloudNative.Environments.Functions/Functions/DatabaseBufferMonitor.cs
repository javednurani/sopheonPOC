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
         [BlobInput(StringConstants.ELASTICPOOL_DATABASE_BUFFER_BLOB_PATH)] string jsonTemplateData, // TODO: Experiment with connection parameter and secrets to always reference azure blob storage
         FunctionContext context)
      {
         ILogger logger = context.GetLogger(nameof(DatabaseBufferMonitor));
         logger.LogInformation($"{nameof(DatabaseBufferMonitor)} TimerTrigger Function executed at: {DateTime.Now}");

         try
         {
            if(string.IsNullOrEmpty(jsonTemplateData))
            {
               throw new ArgumentNullException("jsonTemplateData", string.Concat(StringConstants.BLOB_FILE_NOT_FOUND, StringConstants.ELASTICPOOL_DATABASE_BUFFER_BLOB_PATH));
            }
            string subscriptionId = Environment.GetEnvironmentVariable("AzSubscriptionId");
            string resourceGroupName = Environment.GetEnvironmentVariable("AzResourceGroupName");
            string sqlServerName = Environment.GetEnvironmentVariable("AzSqlServerName");
            string adminLoginEnigma = _configuration["SqlServerAdminEnigma"]; // Pull admin enigma from app config (user secrets or key vault)

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
