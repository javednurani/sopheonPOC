using System;
using System.Collections.Generic;
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
      /// Soft Deletes an Environment
      /// </summary>
      /// <param name="environment">Environment entity with EnvironmentKey, to be soft deleted</param>
      /// <returns>Task</returns>
      /// <exception cref="EntityNotFoundException">Environment entity not found by EnvironmentKey</exception>"
      Task DeleteEnvironment(Guid environmentKey);

      /// <summary>
      /// Updates an Environment
      /// </summary>
      /// <param name="environment">Environment model to be updated</param>
      /// <returns>Task<Environment>, the updated Environment</returns>
      /// <exception cref="EntityNotFoundException">Environment entity not found by EnvironmentKey</exception>"
      Task<Environment> UpdateEnvironment(Environment environment);
   }
}
