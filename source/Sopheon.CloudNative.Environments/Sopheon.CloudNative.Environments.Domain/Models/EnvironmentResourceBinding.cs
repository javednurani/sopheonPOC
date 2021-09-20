namespace Sopheon.CloudNative.Environments.Domain.Models
{
   public class EnvironmentResourceBinding : Entity
   {
      public int EnvironmentId
      {
         get;
         set;
      }

      public virtual Environment Environment
      {
         get;
         set;
      }

      public int ResourceId
      {
         get;
         set;
      }

      public virtual Resource Resource
      {
         get;
         set;
      }

      public int BusinessServiceDependencyId
      {
         get;
         set;
      }

      public virtual BusinessServiceDependency BusinessServiceDependency
      {
         get;
         set;
      }
   }
}
