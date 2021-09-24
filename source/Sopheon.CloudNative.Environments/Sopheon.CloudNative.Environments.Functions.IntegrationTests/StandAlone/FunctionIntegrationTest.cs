using System.Net.Http;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests.StandAlone
{
   public abstract class FunctionIntegrationTest
   {
      protected readonly Environments_OpenApiClient _sut = new Environments_OpenApiClient(new HttpClient());
   }
}