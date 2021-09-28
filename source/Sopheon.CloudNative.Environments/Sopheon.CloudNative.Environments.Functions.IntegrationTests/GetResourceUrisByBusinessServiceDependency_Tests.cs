using System.Collections.Generic;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests
{
   public class GetResourceUrisByBusinessServiceDependency_Tests : FunctionIntegrationTest
   {
      [FunctionFact]
      public async void HappyPath_GetResourceUrisByBusinessServiceDependency()
      {
         // TODO replace with actual seed data
         ICollection<ResourceUriDto> results = await _sut.GetResourceUrisByBusinessServiceDependencyAsync("PRODUCT_SERVICE", "PRODUCT_DATASTORE");
         Assert.NotNull(results);
         Assert.True(results.Count == 2); // TODO adjust per actual seed data
      }
   }
}
