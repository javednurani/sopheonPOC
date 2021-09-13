﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Data
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
         return await _context.Environments
            .Where(env => !env.IsDeleted)
            .AsNoTracking()
            .ToArrayAsync();
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

      public async Task<Environment> UpdateEnvironment(Environment environment)
      {
         Environment entityEnvironment = await _context.Environments.SingleOrDefaultAsync(env => !env.IsDeleted && env.EnvironmentKey == environment.EnvironmentKey);

         if(entityEnvironment == null)
         {
            throw new EntityNotFoundException($"An Environment was not found with a key: {environment.EnvironmentKey}");
         }

         entityEnvironment.Name = environment.Name;
         entityEnvironment.Owner = environment.Owner;
         entityEnvironment.Description = environment.Description;

         Environment newEnvironment = _context.Environments.Update(entityEnvironment).Entity;
         await _context.SaveChangesAsync();

         return newEnvironment;
      }
   }
}
