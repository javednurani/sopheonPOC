using System.Collections.Generic;
using Sopheon.CloudNative.Environments.Utility;
using Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.DataDependent
{
   public class GetResourceUrisByBusinessServiceDependency_Tests : DataDependentFunctionIntegrationTest
   {
      [DataDependentFunctionFact]
      public async void HappyPath_GetResourceUrisByBusinessServiceDependency()
      {
         ICollection<ResourceUriDto> results = await _sut.GetResourceUrisByBusinessServiceDependencyAsync(TestData.BUSINESS_SERVICE_NAME_1, TestData.DEPENDENCY_NAME_1);
         Assert.NotNull(results);
         Assert.True(results.Count == 2);
         Assert.Contains(results, r => r.Uri == TestData.RESOURCE_URI_1);
      }
   }
}
