
namespace Sopheon.CloudNative.Environments.Data.EntityConfigurations
{
   public class BaseConfiguration
   {
      public static string GetIdColumnName<T>()
      {
         return typeof(T).Name + "Id";
      }
   }
}
