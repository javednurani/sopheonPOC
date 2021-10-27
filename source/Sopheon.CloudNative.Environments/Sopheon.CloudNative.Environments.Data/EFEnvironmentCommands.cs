using System;
using System.Threading.Tasks;
using Sopheon.CloudNative.Environments.Data.Extensions;
using Sopheon.CloudNative.Environments.Domain.Commands;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Data
{
   public class EFEnvironmentCommands : IEnvironmentCommands
   {
      private readonly EnvironmentContext _context;

      public EFEnvironmentCommands(EnvironmentContext context)
      {
         _context = context;
      }

      public async Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(Guid environmentKey)
      {
         Environment entityEnvironment = await _context.Environments.SingleEnvironmentAsync(environmentKey);
      }
   }
}
