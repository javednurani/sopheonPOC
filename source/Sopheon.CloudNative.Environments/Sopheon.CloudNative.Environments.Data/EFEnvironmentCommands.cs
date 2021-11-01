using System;
using System.Threading.Tasks;
using Sopheon.CloudNative.Environments.Data.Extensions;
using Sopheon.CloudNative.Environments.Domain.Commands;
using Sopheon.CloudNative.Environments.Domain.Enums;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Domain.Models;
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

      public async Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(Guid environmentKey, string resourceUri)
      {
         Resource azureSqlDatabaseResource = new Resource
         {
            DomainResourceTypeId = (int)ResourceTypes.AzureSqlDb,
            Uri = resourceUri
         };

         try
         {
            Environment entityEnvironment = await _context.Environments.SingleEnvironmentAsync(environmentKey);

            DedicatedEnvironmentResource dedicatedEnvironmentResource = new DedicatedEnvironmentResource
            {
               Environment = entityEnvironment,
               Resource = azureSqlDatabaseResource
            };

            _context.DedicatedEnvironmentResources.Add(dedicatedEnvironmentResource);
            await _context.SaveChangesAsync();
         }
         catch (Exception ex) // TODO only catch SqlExceptions thrown by the specific constraint violations
         {
            throw new CommandFailedException("TODO");
         }
      }
   }
}
