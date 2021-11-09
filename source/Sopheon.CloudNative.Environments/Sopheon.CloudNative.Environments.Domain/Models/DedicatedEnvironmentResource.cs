namespace Sopheon.CloudNative.Environments.Domain.Models
{
   public class DedicatedEnvironmentResource : Entity
   {
      public int EnvironmentId
      {
         get;
         set;
      }

      public int ResourceId
      {
         get;
         set;
      }

      public virtual Environment Environment
      {
         get;
         set;
      }

      public virtual Resource Resource
      {
         get;
         set;
      }
   }
}
