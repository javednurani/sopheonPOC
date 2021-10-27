using System;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Environments.Functions.Helpers
{
   public interface IResourceAllocatorHelper
   {
      Task AllocateResourcesForEnvironment(Guid environmentKey);
   }
}
