using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace Sopheon.CloudNative.Products.Functions
{
   public class AuthenticationMiddleware : IFunctionsWorkerMiddleware
   {
      private readonly AuthenticationProvider _authenticationProvider;

      public AuthenticationMiddleware(AuthenticationProvider authenticationProvider)
      {
         _authenticationProvider = authenticationProvider;
      }

      public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
      {
         //var logger = context.GetLogger<AuthenticationMiddleware>();
         IReadOnlyDictionary<string, object> data = context.BindingContext.BindingData;

         if (data.TryGetValue("Headers", out object headersObject))
         {
            JsonDocument headers = JsonSerializer.Deserialize<JsonDocument>(headersObject as string);

            if (headers.RootElement.TryGetProperty("Authorization", out JsonElement authorizationHeader))
            {
               string token = authorizationHeader.ToString().Substring("Bearer ".Length);

               ClaimsPrincipal principal = await _authenticationProvider.AuthenticateAsync(context, token);
               if (principal != null)
               {
                  await next(context);
                  return;
               }
            }
         }

         // return 401
      }
   }
}
