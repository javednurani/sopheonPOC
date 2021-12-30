using System;
using System.Threading.Tasks;
using Sopheon.CloudNative.Environments.Domain.Models;

namespace Sopheon.CloudNative.Environments.Domain.Commands
{
   public interface IEnvironmentCommands
   {
      Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(Guid environmentKey, Resource resource);
   }
}
