using Microsoft.Azure.Functions.Worker;

namespace Sopheon.CloudNative.Products.Functions
{
   public class FunctionContextConnectionStringProvider : ITenantEnvironmentConnectionStringProvider
   {
      private readonly IEnvironmentConnectionAuthorizer<FunctionContext> _functionContextEnvironmentAuthorizer;

      public FunctionContextConnectionStringProvider(IEnvironmentConnectionAuthorizer<FunctionContext> functionContextEnvironmentAuthorizer)
      {
         _functionContextEnvironmentAuthorizer = functionContextEnvironmentAuthorizer;
      }

      public string GetConnectionString()
      {
         return _functionContextEnvironmentAuthorizer.GetEnvironmentConnectionString();
      }
   }
}
