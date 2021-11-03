using System.Threading.Tasks;
using Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure;
using Sopheon.CloudNative.Environments.Utility.TestData;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.DataDependent
{
   public class AllocateSqlDatabaseSharedByServicesToEnvironment_Tests : DataDependentFunctionIntegrationTest
   {
      [DataDependentFunctionFact (Skip = "Test triggers Tagging of Azure SQL Databases, and is not re-runnable without ENV 'delete allocated resources' behavior")]
      public async Task AllocateSqlDatabaseSharedByServicesToEnvironment_HappyPath()
      {
         ResourceAllocationResponseDto result = await _sut.AllocateSqlDatabaseSharedByServicesToEnvironmentAsync(TestDataConstants.EnvironmentKey3);
         Assert.NotNull(result);
      }
   }
}
