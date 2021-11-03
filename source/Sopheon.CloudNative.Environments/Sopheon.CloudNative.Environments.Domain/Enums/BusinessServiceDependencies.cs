using Sopheon.CloudNative.Environments.Domain.Infrastructure;

namespace Sopheon.CloudNative.Environments.Domain.Enums
{
   /// <summary>
   /// Do not change these values; they are used to generate seed data
   /// </summary>
   public enum BusinessServiceDependencies
   {
      [BusinessService(BusinessServices.ProductManagement)]
      [ResourceType(ResourceTypes.AzureSqlDb)]
      ProductManagementSqlDb = 1,
   }
}
