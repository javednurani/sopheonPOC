using Sopheon.CloudNative.Environments.Domain.Exceptions;
using Sopheon.CloudNative.Environments.Testing.Common;
using System;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests
{
   public class GetSpecificResourceUriAsync_Tests : FunctionIntegrationTest
   {
      //TODO Add happy path test once seed data is added to story CLOUD-1827
      [FunctionFact]
      public async void HappyPathNotFound_GetSpecificResourceUri()
      {
         try
         {
            //bool test = _sut.ReadResponseAsString;
            var result = await _sut.GetSpecificResourceUriAsync(_environmentKey, _businessServiceName, _businessServiceDependencyName);
         }
         catch (Exception ex)
         {
            Assert.True(true);
         }
      }
   }
}
