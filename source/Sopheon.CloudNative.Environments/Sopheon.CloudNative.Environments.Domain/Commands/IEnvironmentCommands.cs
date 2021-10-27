using System;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Environments.Domain.Commands
{
   public interface IEnvironmentCommands
   {
      Task AllocateResourcesForEnvironment(Guid environmentKey);
   }
}
