using System;
using Sopheon.CloudNative.Environments.Functions.IntegrationTests.Infrastructure;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.DataDependent
{
   public class GetSpecificResourceUriAsync_Tests : DataDependentFunctionIntegrationTest
   {
      //TODO Add happy path test once seed data is added to story CLOUD-1827
      [DataDependentFunctionFact]
      public async void HappyPathNotFound_GetSpecificResourceUri()
      {
         try
         {
            var result = await _sut.GetSpecificResourceUriAsync(_environmentKey, _businessServiceName, _businessServiceDependencyName);
         }
         catch (Exception ex)
         {
         }
      }


   }
}
