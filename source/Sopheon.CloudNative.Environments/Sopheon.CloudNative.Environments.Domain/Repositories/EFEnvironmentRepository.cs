using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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

      public async Task<IEnumerable<Environment>> GetEnvironments()
      {
         return await _context.Environments.Where(env => !env.IsDeleted).ToArrayAsync();
      }

      public async Task<bool> DeleteEnvironment(Environment environment)
      {
         Environment entityEnvironment = await _context.Environments.SingleOrDefaultAsync(env => env.EnvironmentKey == environment.EnvironmentKey);

         if (entityEnvironment == null)
         {
            return false;
            //throw new Exception(); - in discussion on UPDATE story
         }

         // TODO: check if entityEnvironment.IsDeleted ?

         entityEnvironment.IsDeleted = true;
         await _context.SaveChangesAsync();
         return true;
      }
   }
}
