using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Sopheon.CloudNative.Products.AspNetCore.Policies.Requirements;

namespace Sopheon.CloudNative.Products.AspNetCore.Policies.Handlers
{
   public class EnvironmentOwnerHandler : AuthorizationHandler<HasRelevantRelationshipToEnvironment>
   {
      private readonly IEnvironmentIdentificationService _environmentIdentificationService;

      public EnvironmentOwnerHandler(IEnvironmentIdentificationService environmentIdentificationService) 
      {
         _environmentIdentificationService = environmentIdentificationService;
      }

      protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
         HasRelevantRelationshipToEnvironment requirement)
      {
         // When using endpoint routing, authorization is typically handled by the Authorization Middleware
         // In this case, the Resource property is an instance of HttpContext.
         // For more information, see https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-5
         if (context.Resource is HttpContext httpContext)
         {
            string environmentId = _environmentIdentificationService.GetEnvironmentIdentifier(httpContext);
            var user = context.User;

            // Short Term: Call to Azure Function to lookup Environment Owner
            // Long Term: Claims for explicit environment access? Will need to evaluate claim revocation scenarios
            context.Succeed(requirement);  // TODO: Check Environment Access against User
         }

         //if (context.User.HasClaim(c => c.Type == "BadgeId" &&
         //                               c.Issuer == "http://microsoftsecurity"))
         //{
         //   context.Succeed(requirement);
         //}

         return Task.CompletedTask;
      }
   }
}
