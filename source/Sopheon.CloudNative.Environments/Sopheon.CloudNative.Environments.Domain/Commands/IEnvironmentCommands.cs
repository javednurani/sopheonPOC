using System;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Environments.Domain.Commands
{
   public interface IEnvironmentCommands
   {
      Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(Guid environmentKey, string resourceUri);
   }
}
