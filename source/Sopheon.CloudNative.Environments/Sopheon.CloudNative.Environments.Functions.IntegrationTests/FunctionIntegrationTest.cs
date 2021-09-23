using System.Net.Http;

namespace Sopheon.CloudNative.Environments.Functions.IntegrationTests
{
   public class FunctionIntegrationTest
   {
      protected readonly Environments_OpenApiClient _sut = new Environments_OpenApiClient(new HttpClient());
   }
}