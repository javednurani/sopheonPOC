using System.Threading.Tasks;
using Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure;
using Sopheon.CloudNative.Environments.Utility;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.DataDependent
{
   public class AllocateSqlDatabaseSharedByServicesToEnvironment_Tests : DataDependentFunctionIntegrationTest
   {
      [DataDependentFunctionFact]
      public async Task AllocateSqlDatabaseSharedByServicesToEnvironment_HappyPath()
      {
         ResourceAllocationResponseDto result = await _sut.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(TestData.EnvironmentKey1);
         Assert.Equal("TODO", result.Message);
      }
   }
}
