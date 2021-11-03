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
         ICollection<ResourceUriDto> results = await _sut.GetResourceUrisByBusinessServiceDependencyAsync(BusinessServices.ProductManagement.ToString(), TestDataConstants.DEPENDENCY_NAME_1);
         Assert.NotNull(results);
         Assert.Equal(2, results.Count); // TODO Cloud-1744, this assert may be compromised by 'Resource Allocation' integration tests
         Assert.Contains(results, r => r.Uri == TestDataConstants.RESOURCE_URI_1);
      }
   }
}
