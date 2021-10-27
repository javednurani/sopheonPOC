using Microsoft.AspNetCore.Http;

namespace Sopheon.CloudNative.Products.AspNetCore
{
   public interface IEnvironmentIdentificationService
   {
      string GetEnvironmentIdentifier(HttpContext httpContext);
   }

   public class EnvironmentIdentificationService : IEnvironmentIdentificationService
   {
      public EnvironmentIdentificationService()
      {
      }

      public string GetEnvironmentIdentifier(HttpContext httpContext)
      {
         return httpContext.Request.RouteValues.TryGetValue("environmentid", out object parameter) ? parameter.ToString() : null;
      }
   }
}
