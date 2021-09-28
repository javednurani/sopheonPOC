using Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure;
using Sopheon.CloudNative.Environments.Utility;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.DataDependent
{
   public class GetSpecificResourceUriAsync_Tests : DataDependentFunctionIntegrationTest
   {
      [DataDependentFunctionFact]
      public async void HappyPath_GetSpecificResourceUri()
      {
         ResourceUriDto result = await _sut.GetSpecificResourceUriAsync(TestData.EnvironmentKey1, TestData.BUSINESS_SERVICE_NAME_1, TestData.DEPENDENCY_NAME_1);
         Assert.NotEmpty(result.Uri);
      }
   }
}
