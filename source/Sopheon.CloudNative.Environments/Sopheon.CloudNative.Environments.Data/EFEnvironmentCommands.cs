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

      public async Task AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(Guid environmentKey, Resource resource)
      {
         try
         {
            Environment environment = await _context.Environments
               .SingleEnvironmentAsync(environmentKey);

            BusinessServiceDependency[] azureSqlDbBusinessServiceDependencies = await _context.BusinessServiceDependencies
               .Where(bsd => bsd.DomainResourceTypeId == (int)ResourceTypes.AzureSqlDb)
               .ToArrayAsync();

            DedicatedEnvironmentResource dedicatedEnvironmentResource = new DedicatedEnvironmentResource
            {
               Environment = environment,
               Resource = resource
            };

            IEnumerable<EnvironmentResourceBinding> environmentResourceBindings = azureSqlDbBusinessServiceDependencies.Select(bsd => new EnvironmentResourceBinding
            {
               BusinessServiceDependency = bsd,
               Environment = environment,
               Resource = resource
            });

            _context.DedicatedEnvironmentResources.Add(dedicatedEnvironmentResource);
            _context.EnvironmentResourceBindings.AddRange(environmentResourceBindings);

            await _context.SaveChangesAsync();
         }
         catch (DbUpdateException ex)
         {
            throw new CommandFailedException($"{nameof(AllocateSqlDatabaseSharedByServicesToEnvironmentAsync)} Command failed", ex);
         }
      }
   }
}
