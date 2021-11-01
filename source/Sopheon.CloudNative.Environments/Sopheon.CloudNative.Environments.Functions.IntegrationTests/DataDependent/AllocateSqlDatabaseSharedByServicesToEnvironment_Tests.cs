using System.Threading.Tasks;
using Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure;
using Sopheon.CloudNative.Environments.Utility.TestData;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.DataDependent
{
   public class AllocateSqlDatabaseSharedByServicesToEnvironment_Tests : DataDependentFunctionIntegrationTest
   {
      [DataDependentFunctionFact (Skip = "Cloud-1960 in progress")]
      public async Task AllocateSqlDatabaseSharedByServicesToEnvironment_HappyPath()
      {
         ResourceAllocationResponseDto result = await _sut.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(TestDataConstants.EnvironmentKey1);
         Assert.NotNull(result);
      }
   }
}
