using System;
using System.Threading.Tasks;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests
{
   public class Environments_HappyPath : FunctionIntegrationTest
   {
      [FunctionFact]
      public async Task HappyPath_AllFunctions()
      {
         // create an environment
         var createDto = new EnvironmentDto { Name = "ZachIntegrationTest", Description = "TestDescription", Owner = Guid.NewGuid() };
         var createdEnv = await _sut.CreateEnvironmentAsync(createDto);

         // update environment
         createdEnv.Name = "ZachIntegrationTest_Updated";
         await _sut.UpdateEnvironmentAsync(createdEnv.EnvironmentKey, createdEnv);

         // delete environment
         await _sut.DeleteEnvironmentAsync(createdEnv.EnvironmentKey);

         // get evironments - verify deleted
         var environments = await _sut.GetEnvironmentsAsync();
         Assert.DoesNotContain(environments, e => e.EnvironmentKey == createdEnv.EnvironmentKey);
      }
   }
}
