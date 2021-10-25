using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;

namespace Sopheon.CloudNative.Products.Functions
{
   public class FunctionContextEnvironmentAuthorizer : IEnvironmentConnectionAuthorizer<FunctionContext>
   {
      private string _connectionString;

      public async Task<bool> AuthorizeEnvironmentAccess(FunctionContext context)
      {
         return TryAuthorizeConnection(context, out _connectionString);
      }

      public string GetEnvironmentConnectionString()
      {
         if (_connectionString == null)
         {
            throw new Exception("Connection details for environment not authorized for function context.");
         }

         return _connectionString;
      }

      private bool TryAuthorizeConnection(FunctionContext context, out string connectionString)
      {
         // Mock out retrieval; this could be a request to an external service
         Dictionary<string, string> catalog = new Dictionary<string, string>
         {
            ["Environment1"] = "Server=vm-box16;Database=FunctionEFTenant1;Integrated Security=True",
            ["Environment2"] = "Server=vm-box16;Database=FunctionEFTenant2;Integrated Security=True"
         };
         return catalog.TryGetValue(context.BindingContext.BindingData["environmentId"] as string, out connectionString);
      }
   }
}
