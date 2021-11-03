using Sopheon.CloudNative.Environments.Domain.Infrastructure;

namespace Sopheon.CloudNative.Environments.Domain.Enums
{
   public enum BusinessServiceDependencies
   {
      [BusinessService(BusinessServices.ProductManagement)]
      [ResourceType(ResourceTypes.AzureSqlDb)]
      ProductManagementSqlDb = 1,
   }
}
