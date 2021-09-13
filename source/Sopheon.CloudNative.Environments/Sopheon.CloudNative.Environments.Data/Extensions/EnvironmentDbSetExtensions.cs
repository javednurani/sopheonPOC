using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Data.Extensions
{
   /// <summary>
   /// The purpose of this class is to consolidate queries (or parts of queries) commonly used with Environments
   /// </summary>
   public static class EnvironmentDbSetExtensions
   {
      /// <summary>
      /// Finds an undeleted Environment by <paramref name="environmentKey"/>, throwing an exception if not found.
      /// </summary>
      /// <param name="environmentSet">The set of Environments to search.</param>
      /// <param name="environmentKey">The key of the desired environment.</param>
      /// <returns>The matching Environment if it exists</returns>
      /// <exception cref="EntityNotFoundException">The requested entity was not found.</exception>
      public static async Task<Environment> SingleEnvironmentAsync(this DbSet<Environment> environmentSet, Guid environmentKey)
      {
         Environment environment = await environmentSet.SingleOrDefaultAsync(env => !env.IsDeleted && env.EnvironmentKey == environmentKey);

         if (environment == null)
         {
            throw new EntityNotFoundException($"An Environment was not found with a key: {environmentKey}");
         }

         return environment;
      }
   }
}
