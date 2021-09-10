using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Domain.Data;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
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
         //return await _context.Environments.ToArrayAsync();
         return await _context.Environments.Where(env => !env.IsDeleted).ToArrayAsync();
      }

      public async Task DeleteEnvironment(Guid environmentKey)
      {
         Environment entityEnvironment = await _context.Environments.SingleOrDefaultAsync(env => env.EnvironmentKey == environmentKey && !env.IsDeleted);

         if (entityEnvironment == null)
         {
            throw new EntityNotFoundException($"An Environment was not found with a key: {environmentKey}");
         }

         entityEnvironment.IsDeleted = true;
         await _context.SaveChangesAsync();
      }
   }
}
