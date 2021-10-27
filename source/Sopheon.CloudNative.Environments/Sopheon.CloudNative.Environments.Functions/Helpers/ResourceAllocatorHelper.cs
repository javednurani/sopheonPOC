using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Environments.Domain.Commands;

namespace Sopheon.CloudNative.Environments.Functions.Helpers
{
   public class ResourceAllocatorHelper : IResourceAllocatorHelper
   {
      private readonly ILogger<ResourceAllocatorHelper> _logger;
      private readonly IEnvironmentCommands _environmentCommands;

      public ResourceAllocatorHelper(ILogger<ResourceAllocatorHelper> logger, IEnvironmentCommands environmentCommands)
      {
         _logger = logger;
         _environmentCommands = environmentCommands;
      }

      public async Task AllocateResourcesForEnvironment(Guid environmentKey)
      {
         await _environmentCommands.AllocateResourcesForEnvironment(environmentKey);
      }
   }
}
