using System.Collections.Generic;
using System.Threading.Tasks;
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
         ICollection<ResourceUriDto> results = await _sut.GetResourceUrisByBusinessServiceDependencyAsync(TestDataConstants.BUSINESS_SERVICE_NAME_1, TestDataConstants.DEPENDENCY_NAME_1);
         Assert.NotNull(results);
         Assert.Equal(1, results.Count);
         Assert.Contains(results, r => r.Uri == TestDataConstants.RESOURCE_URI_1);
      }
   }
}
