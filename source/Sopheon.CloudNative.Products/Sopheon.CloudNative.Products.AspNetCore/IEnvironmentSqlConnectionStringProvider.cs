using System.Threading.Tasks;

namespace Sopheon.CloudNative.Products.AspNetCore
{
   public interface IEnvironmentSqlConnectionStringProvider
   {
      Task<string> GetConnectionStringAsync();
   }
}
