
namespace Sopheon.CloudNative.Environments.Data.EntityConfigurations
{
   public class BaseConfiguration
   {
      public string GetEntityId()
      {
         return this.GetType().Name +"Id";
      }
   }
}
