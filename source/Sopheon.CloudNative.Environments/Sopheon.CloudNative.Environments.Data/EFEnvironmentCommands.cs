using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
         try
         {
            Resource azureSqlDatabaseResource = new Resource
            {
               DomainResourceTypeId = (int)ResourceTypes.AzureSqlDb,
               Uri = resourceUri
            };

            Environment environment = await _context.Environments.SingleEnvironmentAsync(environmentKey);
            BusinessServiceDependency[] businessServiceDependencies = await _context.BusinessServiceDependencies.ToArrayAsync();

            DedicatedEnvironmentResource dedicatedEnvironmentResource = new DedicatedEnvironmentResource
            {
               Environment = environment,
               Resource = azureSqlDatabaseResource
            };

            IEnumerable<EnvironmentResourceBinding> environmentResourceBindings = businessServiceDependencies.Select(bsd => new EnvironmentResourceBinding
            {
               BusinessServiceDependency = bsd,
               Environment = environment,
               Resource = azureSqlDatabaseResource
            });

            _context.DedicatedEnvironmentResources.Add(dedicatedEnvironmentResource);
            _context.EnvironmentResourceBindings.AddRange(environmentResourceBindings);

            await _context.SaveChangesAsync();
         }
         catch (DbUpdateException ex)
         {
            throw new CommandFailedException($"{nameof(AllocateSqlDatabaseSharedByServicesToEnvironmentAsync)} EF Command failed", ex);
         }
      }
   }
}
