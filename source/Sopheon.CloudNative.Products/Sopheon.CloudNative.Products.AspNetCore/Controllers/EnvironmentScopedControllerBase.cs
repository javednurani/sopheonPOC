using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sopheon.CloudNative.Products.AspNetCore.Policies;

namespace Sopheon.CloudNative.Products.AspNetCore.Controllers
{
   [ApiController]
   [Route("Environments/{EnvironmentId}/[controller]")]
   [Authorize(Policy = nameof(HasEnvironmentAccessPolicy))]
   public abstract class EnvironmentScopedControllerBase : ControllerBase
   {
   }
}
