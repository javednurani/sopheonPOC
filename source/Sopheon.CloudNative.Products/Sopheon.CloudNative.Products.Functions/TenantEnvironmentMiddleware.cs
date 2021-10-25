using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace Sopheon.CloudNative.Products.Functions
{
   public class TenantEnvironmentMiddleware : IFunctionsWorkerMiddleware
   {
      private readonly AzureAdJwtBearerValidation _azureADJwtBearerValidation;

      public TenantEnvironmentMiddleware(AzureAdJwtBearerValidation azureADJwtBearerValidation) 
      {
         _azureADJwtBearerValidation = azureADJwtBearerValidation;
      }

      public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
      {
         // TODO: add authentication check + check access against requested environment
         // https://joonasw.net/view/azure-ad-jwt-authentication-in-net-isolated-process-azure-functions?hmsr=joyk.com&utm_source=joyk.com&utm_medium=referral
         // https://github.com/hajekj/azure-functions-dotnet-worker-miw
         // https://damienbod.com/2020/09/24/securing-azure-functions-using-azure-ad-jwt-bearer-token-authentication-for-user-access-tokens/

         // This is added pre-function execution, function will have access to this information
         // in the context.Items dictionary
         //context.Items.Add("middlewareitem", "Hello, from middleware");
         var environmentConnectionAuthorizer = context.InstanceServices.GetService(typeof(IEnvironmentConnectionAuthorizer<FunctionContext>)) as IEnvironmentConnectionAuthorizer<FunctionContext>;

         bool authorized = await environmentConnectionAuthorizer.AuthorizeEnvironmentAccess(context); // implementation variable: can use token claims, redis cache of available environments, etc.

         if (!authorized)
         {
            context.GetLogger<TenantEnvironmentMiddleware>().LogError("Failed to authorize environment access", context.BindingContext.BindingData);
            // TODO: Exit and return 400 HTTP Response here
            // Currently there isn't a recommended implementation of this behavior for .NET 5 out of process Azure Functions
         }

         await next(context);

         // This happens after function execution. We can inspect the context after the function
         // was invoked
         if (context.Items.TryGetValue("functionitem", out object value) && value is string message)
         {
            ILogger logger = context.GetLogger<TenantEnvironmentMiddleware>();

            logger.LogInformation("From function: {message}", message);
         }
      }
   }
}
