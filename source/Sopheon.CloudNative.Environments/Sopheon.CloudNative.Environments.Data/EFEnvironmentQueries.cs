using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Domain.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Environments.Data
{
   public class EFEnvironmentQueries : IEnvironmentQueries
   {
      private readonly EnvironmentContext _context;

      public EFEnvironmentQueries(EnvironmentContext context)
      {
         _context = context;
      }

      public async Task<IEnumerable<string>> GetResourceUrisByBusinessServiceDependency(string businessServiceName, string dependencyName)
      {
         var businessService = await _context.BusinessServices.FirstOrDefaultAsync(
            bs => bs.Name.Equals(businessServiceName));

         if (businessService == null)
         {
            throw new EntityNotFoundException($"An BusinessService was not found with a name: {businessServiceName}");
         }

         // explore nav props instead of multiple queries

         var businessServiceDependency = await _context.BusinessServiceDependencies.FirstOrDefaultAsync(
            bsd => bsd.BusinessServiceId == businessService.Id
               && bsd.DependencyName.Equals(dependencyName));

         if (businessServiceDependency == null)
         {
            throw new EntityNotFoundException($"An BusinessServiceDependency for the service: {businessServiceName} was not found with a name: {dependencyName}");
         }

         var environmentResourceBindings = _context.EnvironmentResourceBindings.Where(
            erb => erb.BusinessServiceDependencyId == businessServiceDependency.Id);

         var resourceUris = environmentResourceBindings.Select(erb => erb.Resource.Uri);

         return await resourceUris.ToArrayAsync();
      }

      public Task<string> GetSpecificResourceUri(string environmentKey, string businessServiceName, string dependencyName)
      {
         throw new System.NotImplementedException();
      }
   }
}
