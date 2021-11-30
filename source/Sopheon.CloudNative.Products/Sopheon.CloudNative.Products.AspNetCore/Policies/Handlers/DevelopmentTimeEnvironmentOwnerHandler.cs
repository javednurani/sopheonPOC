using Microsoft.AspNetCore.Authorization;
using Sopheon.CloudNative.Products.AspNetCore.Policies.Requirements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Products.AspNetCore.Policies.Handlers
{
    public class DevelopmentTimeEnvironmentOwnerHandler : AuthorizationHandler<HasRelevantRelationshipToEnvironment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasRelevantRelationshipToEnvironment requirement)
        {
            // In a regular development environment, assume the currently logged in user is the owner of the environment.
            // To turn this off, ensure UseEnvironmentDatabasesFromAppSettings setting is set to false and set up local environments service.

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
