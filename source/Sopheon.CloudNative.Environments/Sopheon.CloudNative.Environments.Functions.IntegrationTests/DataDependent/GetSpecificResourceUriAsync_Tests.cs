using System.Threading.Tasks;
using Sopheon.CloudNative.Environments.Domain.Enums;
using Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure;
using Sopheon.CloudNative.Environments.Utility.TestData;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.DataDependent
{
   public class GetSpecificResourceUriAsync_Tests : DataDependentFunctionIntegrationTest
   {
      [DataDependentFunctionFact]
      public async Task GetSpecificResourceUri_HappyPath()
      {
         ResourceUriDto result = await _sut.GetSpecificResourceUriAsync(TestDataConstants.EnvironmentKey1, BusinessServices.ProductManagement.ToString(), TestDataConstants.DEPENDENCY_NAME_1);
         Assert.NotEmpty(result.Uri);
      }
   }
}
