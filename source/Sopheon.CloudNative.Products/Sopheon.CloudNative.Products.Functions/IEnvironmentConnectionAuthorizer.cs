using System.Threading.Tasks;

namespace Sopheon.CloudNative.Products.Functions
{
   public interface IEnvironmentConnectionAuthorizer<TContext>
   {
      /// <summary>
      /// May be an async operation
      /// </summary>
      /// <param name="context"></param>
      /// <returns></returns>
      Task<bool> AuthorizeEnvironmentAccess(TContext context);

      string GetEnvironmentConnectionString();
   }
}
