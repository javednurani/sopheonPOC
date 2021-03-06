using System.Collections.Generic;
using System.Threading.Tasks;
using Sopheon.CloudNative.Environments.Domain.Enums;
using Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure;
using Sopheon.CloudNative.Environments.Utility.TestData;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.DataDependent
{
   public class GetResourceUrisByBusinessServiceDependency_Tests : DataDependentFunctionIntegrationTest
   {
      [DataDependentFunctionFact]
      public async Task GetResourceUrisByBusinessServiceDependency_HappyPath()
      {
         ICollection<ResourceUriDto> results = await _sut.GetResourceUrisByBusinessServiceDependencyAsync(BusinessServices.ProductManagement.ToString(), BusinessServiceDependencies.ProductManagementSqlDb.ToString());
         Assert.NotNull(results);
         Assert.Equal(2, results.Count);
         Assert.Contains(results, r => r.Uri == TestDataConstants.RESOURCE_URI_1);
      }
   }
}
