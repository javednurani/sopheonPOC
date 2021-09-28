using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Testing.Common;
using System;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests
{
   public class GetSpecificResourceUriAsync_Tests : FunctionIntegrationTest
   {
      [FunctionFact]
      public async void HappyPath_GetSpecificResourceUri()
      {
         // TODO replace with actual seed data
         ResourceUriDto result = await _sut.GetSpecificResourceUriAsync(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Demo Business Service", "Demo Dependency Name");
         Assert.NotEmpty(result.Uri);
      }
   }
}
