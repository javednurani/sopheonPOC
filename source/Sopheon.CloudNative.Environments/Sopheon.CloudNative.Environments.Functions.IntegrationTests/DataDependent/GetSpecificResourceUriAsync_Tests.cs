using System;
using Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.DataDependent
{
   public class GetSpecificResourceUriAsync_Tests : DataDependentFunctionIntegrationTest
   {
      [DataDependentFunctionFact]
      public async void HappyPath_GetSpecificResourceUri()
      {
         // TODO replace with actual seed data
         ResourceUriDto result = await _sut.GetSpecificResourceUriAsync(Guid.Parse("11111111-1111-1111-1111-111111111111"), "COMMENT_SERVICE", "COMMENT_DATASTORE");
         Assert.NotEmpty(result.Uri);
      }
   }
}
