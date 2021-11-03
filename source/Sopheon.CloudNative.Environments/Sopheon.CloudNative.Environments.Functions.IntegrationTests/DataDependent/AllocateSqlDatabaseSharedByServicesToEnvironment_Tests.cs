using System.Threading.Tasks;
using Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure;
using Sopheon.CloudNative.Environments.Utility.TestData;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.DataDependent
{
   public class AllocateSqlDatabaseSharedByServicesToEnvironment_Tests : DataDependentFunctionIntegrationTest
   {
      [DataDependentFunctionFact (Skip = "TODO Cloud-1744, test is not re-runnable without 'delete allocated resources' functionality")]
      public async Task AllocateSqlDatabaseSharedByServicesToEnvironment_HappyPath()
      {
         ResourceAllocationResponseDto result = await _sut.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(TestDataConstants.EnvironmentKey3);
         Assert.NotNull(result);
      }
   }
}
