using Sopheon.CloudNative.Environments.Domain.Infrastructure;

namespace Sopheon.CloudNative.Environments.Domain.Enums
{
   /// <summary>
   /// Do not change these values; they are used to generate seed data
   /// </summary>
   public enum ResourceTypes
   {
      [Dedicated(true)]
      AzureSqlDb = 1,
      [Dedicated(false)]
      AzureBlobStorage = 2,
   }
}
