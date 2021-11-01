using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Environments.Domain.Commands;

namespace Sopheon.CloudNative.Environments.Functions.Helpers
{
   public class ResourceAllocationHelper : IResourceAllocationHelper
   {
      private readonly ILogger<ResourceAllocationHelper> _logger;
      private readonly IEnvironmentCommands _environmentCommands;

      public ResourceAllocationHelper(ILogger<ResourceAllocationHelper> logger, IEnvironmentCommands environmentCommands)
      {
         _logger = logger;
         _environmentCommands = environmentCommands;
      }

      public async Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(Guid environmentKey)
      {
         _logger.LogInformation($"Executing {nameof(AllocateSqlDatabaseSharedByServicesToEnvironmentAsync)}");
         await _environmentCommands.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(environmentKey, string.Empty);
      }
   }
}
