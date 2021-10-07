using System.Threading.Tasks;

namespace Sopheon.CloudNative.Environments.Functions.Helpers
{
   public interface IDatabaseBufferMonitorHelper
   {
      /// <summary>
      /// Query azure to see the avaiable, unavailable, and provisining databases.
      /// Check to see if unavailable + provisioning >= threshold value
      /// </summary>
      /// <param name="subscriptionId">Id of the subscription</param>
      /// <param name="resourceGroupName">Id of the ResourceGroup</param>
      /// <param name="sqlServerName">Id of the SQL Server</param>
      /// <param name="deploymentTemplateJson">Template to use if deployment is required</param>
      /// <returns>true, if the number of database available meets or exceeds the threshold</returns>
      Task<bool> CheckHasDatabaseThreshold(string subscriptionId, string resourceGroupName, string sqlServerName, string deploymentTemplateJson);
   }
}
