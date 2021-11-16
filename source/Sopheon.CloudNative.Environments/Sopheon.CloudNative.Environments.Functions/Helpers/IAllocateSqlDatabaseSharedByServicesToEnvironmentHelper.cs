using System;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Environments.Functions.Helpers
{
   public interface IAllocateSqlDatabaseSharedByServicesToEnvironmentHelper
   {
      Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(Guid environmentKey, string subscriptionId, string resourceGroupName, string sqlServerName);
   }
}
