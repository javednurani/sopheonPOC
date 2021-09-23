using System.Collections.Generic;

namespace Sopheon.CloudNative.Environments.Domain.Models
{
   public class Resource : Entity
   {
      public int ResourceTypeId
      {
         get;
         set;
      }

      public virtual ResourceType ResourceType
      {
         get;
         set;
      }

      public string Uri
      {
         get;
         set;
      }

      public virtual ICollection<EnvironmentResourceBinding> EnvironmentResourceBindings
      {
         get;
         set;
      }
   }
}
