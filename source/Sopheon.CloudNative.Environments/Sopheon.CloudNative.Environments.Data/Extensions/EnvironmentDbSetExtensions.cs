using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Data.Extensions
{
   /// <summary>
   /// The purpose of this class is to consolidate queries (or parts of queries) commonly used with Environments
   /// </summary>
   public static class EnvironmentDbSetExtensions
   {
      /// <summary>
      /// Finds an undeleted Environment by <paramref name="environmentKey"/>, or null if not found.
      /// </summary>
      /// <param name="environmentSet">The set of Environments to search.</param>
      /// <param name="environmentKey">The key of the desired environment.</param>
      /// <returns>The matching Environment if it exists, null otherwise.</returns>
      public static async Task<Environment> FindEnvironmentAsync(this DbSet<Environment> environmentSet, Guid environmentKey)
      {
         return await environmentSet.SingleOrDefaultAsync(env => !env.IsDeleted && env.EnvironmentKey == environmentKey);
      }
   }
}
