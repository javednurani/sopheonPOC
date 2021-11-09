using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.StandAlone
{
   public class Environments_HappyPath : FunctionIntegrationTest
   {
      [FunctionFact]
      public async Task HappyPath_AllFunctions()
      {
         Guid ownerKey = Some.Random.Guid();
         // create an environment
         EnvironmentDto createDto = new EnvironmentDto 
         { 
            Name = Some.Random.String(), 
            Description = Some.Random.String(), 
            Owner = ownerKey
         };
         EnvironmentDto createdEnv = await _sut.CreateEnvironmentAsync(createDto);

         // update environment
         createdEnv.Name = Some.Random.String();
         await _sut.UpdateEnvironmentAsync(createdEnv.EnvironmentKey, createdEnv);

         // delete environment
         await _sut.DeleteEnvironmentAsync(createdEnv.EnvironmentKey);

         // get evironments - verify deleted
         ICollection<EnvironmentDto> environments = await _sut.GetEnvironmentsAsync(ownerKey);
         Assert.DoesNotContain(environments, e => e.EnvironmentKey == createdEnv.EnvironmentKey);
      }
   }
}
