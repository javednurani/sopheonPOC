﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Domain.Repositories
{
   public interface IEnvironmentRepository
   {
      /// <summary>
      /// Adds an Environment
      /// </summary>
      /// <param name="environment">Environment model to be added</param>
      /// <returns>Task<Guid>, where Guid is the new EnvironmentKey</returns>
      Task<Environment> AddEnvironment(Environment environment);

      /// <summary>
      /// Get a list of all Environments
      /// </summary>
      /// <returns>Task<IEnumerable<Environment>>, a list of all environemts.</returns>
      Task<IEnumerable<Environment>> GetEnvironments();

      /// <summary>
      /// Updates an Environment
      /// </summary>
      /// <param name="environment">Environment model to be updated</param>
      /// <returns>Task<Environment>, the updated Environment</returns>
      Task<Environment> UpdateEnvironment(Environment environment);
   }
}
