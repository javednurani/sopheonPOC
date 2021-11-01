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
      // Azure SQL DB Allocation writes 2 ENV DB records: ENV.Resources record & ENV.DedicatedEnvironmentResources record
      private const int RESOURCE_RECORDS_WRITTEN = 2;

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

            int entriesWritten = await _context.SaveChangesAsync();

            // expect to write an ENV.EnvironmentResourceBinding record per BusinessServiceDepency plus Resource records
            int expectedNumberOfEntries = businessServiceDependencies.Length + RESOURCE_RECORDS_WRITTEN;

            if (entriesWritten != expectedNumberOfEntries) // TODO, is this valuable, (or valid?)
            {
               throw new CommandFailedException($"{nameof(AllocateSqlDatabaseSharedByServicesToEnvironmentAsync)} EF Command did not write expected number of entries to DB.");
            }
         }
         catch (DbUpdateException ex)
         {
            throw new CommandFailedException($"{nameof(AllocateSqlDatabaseSharedByServicesToEnvironmentAsync)} EF Command failed", ex);
         }
      }
   }
}
