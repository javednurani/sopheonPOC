using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

namespace Sopheon.CloudNative.Environments.Testing.Common
{
   /// <summary>
   /// The purpose of this class is to encapsulate generating random entities that are specific to the
   /// 'Sopheon.CloudNative.Environments' namespace.
   /// </summary>
   public static class SomeRandomExtensions
   {
      public static Environment Environment(this SomeRandom someRandom, bool isDeleted = false)
      {
         return new Environment
         {
            EnvironmentKey = someRandom.Guid(),
            EnvironmentID = someRandom.Int(),
            Owner = someRandom.Guid(),
            Name = someRandom.String(),
            Description = someRandom.String(),
            IsDeleted = isDeleted
         };
      }
   }
}
