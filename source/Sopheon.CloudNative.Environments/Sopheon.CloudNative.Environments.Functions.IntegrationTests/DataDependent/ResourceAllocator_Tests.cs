using System.Threading.Tasks;
using Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure;
using Sopheon.CloudNative.Environments.Utility;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.DataDependent
{
   public class ResourceAllocator_Tests : DataDependentFunctionIntegrationTest
   {
      [DataDependentFunctionFact]
      public async Task ResourceAllocator_HappyPath() // TODO review integration test names
      {
         ResourceAllocatorResponseDto result = await _sut.ResourceAllocatorAsync(TestData.EnvironmentKey1);
         Assert.Equal("steel thread", result.Message);
      }
   }
}
