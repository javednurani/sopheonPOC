using Sopheon.CloudNative.Environments.Utility;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests
{
   public class GetSpecificResourceUriAsync_Tests : FunctionIntegrationTest
   {
      [FunctionFact]
      public async void HappyPath_GetSpecificResourceUri()
      {
         ResourceUriDto result = await _sut.GetSpecificResourceUriAsync(TestData.EnvironmentKey1, TestData.BUSINESS_SERVICE_NAME_1, TestData.DEPENDENCY_NAME_1);
         Assert.NotEmpty(result.Uri);
         Assert.Equal(TestData.RESOURCE_URI_1, result.Uri);
      }
   }
}
