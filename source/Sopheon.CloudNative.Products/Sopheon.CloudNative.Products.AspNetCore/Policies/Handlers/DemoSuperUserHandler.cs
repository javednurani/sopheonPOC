using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Sopheon.CloudNative.Products.AspNetCore.Policies.Requirements;

namespace Sopheon.CloudNative.Products.AspNetCore.Policies.Handlers
{
   public class DemoSuperUserHandler : AuthorizationHandler<HasRelevantRelationshipToEnvironment>
   {
      private readonly IConfiguration _configRoot;

      public DemoSuperUserHandler(
         IConfiguration configRoot)
      {
         _configRoot = configRoot;
      }

      protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
         HasRelevantRelationshipToEnvironment requirement)
      {
         // When using endpoint routing, authorization is typically handled by the Authorization Middleware
         // In this case, the Resource property is an instance of HttpContext.
         // For more information, see https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-5
         if (context.Resource is HttpContext httpContext)
         {
            IEnumerable<string> demoSuperUsers = _configRoot.GetSection("DevelopmentAndDemoSettings:DemoSuperUsers").Get<string[]>() ?? Enumerable.Empty<string>();

            ClaimsPrincipal user = context.User;
            string userIdClaimType = _configRoot.GetValue<string>("AzureAdB2C:OwnerIdClaim") ?? string.Empty;
            Claim? userIdClaim = user.FindFirst(claim => userIdClaimType.Equals(claim.Type, System.StringComparison.OrdinalIgnoreCase));

            if (userIdClaim != null && demoSuperUsers.Any(demoSuperUserId => demoSuperUserId.Equals(userIdClaim.Value, System.StringComparison.OrdinalIgnoreCase)))
            {
               context.Succeed(requirement);
            }
         }
      }
   }
}
