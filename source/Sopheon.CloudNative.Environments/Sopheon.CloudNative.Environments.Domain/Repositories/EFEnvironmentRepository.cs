using Sopheon.CloudNative.Environments.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Domain.Repositories
{
   public class EFEnvironmentRepository : IEnvironmentRepository
   {
      private readonly EnvironmentContext _context;

      public EFEnvironmentRepository(EnvironmentContext context)
      {
         _context = context;
      }

      public async Task<Environment> AddEnvironment(Environment environment)
      {
         environment.EnvironmentKey = Guid.NewGuid();

         _context.Environments.Add(environment);
         await _context.SaveChangesAsync();
         return environment;
      }
   }
}
