using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Sopheon.CloudNative.Products.AspNetCore.Policies.Requirements;

namespace Sopheon.CloudNative.Products.AspNetCore.Policies.Handlers
{
   public class EnvironmentOwnerHandler : AuthorizationHandler<HasRelevantRelationshipToEnvironment>
   {
      private readonly IEnvironmentIdentificationService _environmentIdentificationService;
      private readonly EnvironmentOwnerLookupService _environmentOwnerLookupService;
      private readonly IConfiguration _configRoot;

      public EnvironmentOwnerHandler(IEnvironmentIdentificationService environmentIdentificationService,
         EnvironmentOwnerLookupService environmentOwnerLookupService,
         IConfiguration configRoot)
      {
         _environmentIdentificationService = environmentIdentificationService;
         _environmentOwnerLookupService = environmentOwnerLookupService;
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
            string environmentId = _environmentIdentificationService.GetEnvironmentIdentifier(httpContext);
            ClaimsPrincipal user = context.User;
            string ownerIdClaimType = _configRoot.GetValue<string>("AzureAdB2C:OwnerIdClaim") ?? string.Empty;

            // Short Term: Call to Azure Function to lookup Environment Owner
            // Long Term: Claims for explicit environment access? Will need to evaluate claim revocation scenarios
            string environmentOwner = await _environmentOwnerLookupService.GetEnvironmentOwnerAsync(environmentId) ?? string.Empty;

            if (user.HasClaim(c => ownerIdClaimType.Equals(c.Type, System.StringComparison.OrdinalIgnoreCase) && environmentOwner.Equals(c.Value, System.StringComparison.OrdinalIgnoreCase)))
            {
               context.Succeed(requirement);
            }
         }
      }
   }
}
