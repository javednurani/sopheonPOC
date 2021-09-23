using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Domain.Queries;

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
         return await _context.EnvironmentResourceBindings
            .Where(erb => erb.BusinessServiceDependency.DependencyName == dependencyName
               && erb.BusinessServiceDependency.BusinessService.Name == businessServiceName)
            .Select(erb => erb.Resource.Uri)
            .ToArrayAsync();
      }

      public async Task<string> GetSpecificResourceUri(Guid environmentKey, string businessServiceName, string dependencyName)
      {
         string resourceUri = await _context.EnvironmentResourceBindings
            .Where(erb => erb.Environment.EnvironmentKey == environmentKey 
            && erb.BusinessServiceDependency.DependencyName == dependencyName
            && erb.BusinessServiceDependency.BusinessService.Name == businessServiceName)
            .Select(erb => erb.Resource.Uri)
            .SingleOrDefaultAsync(); 

         if(string.IsNullOrEmpty(resourceUri))
         {
            throw new EntityNotFoundException();
         }

         return resourceUri;
      }
   }
}
