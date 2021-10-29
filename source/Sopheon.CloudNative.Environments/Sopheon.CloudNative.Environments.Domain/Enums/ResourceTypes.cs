using Sopheon.CloudNative.Environments.Domain.Infrastructure;

namespace Sopheon.CloudNative.Environments.Domain.Enums
{
   public enum ResourceTypes
   {
      [Dedicated(true)]
      AzureSqlDb = 1
   }
}
