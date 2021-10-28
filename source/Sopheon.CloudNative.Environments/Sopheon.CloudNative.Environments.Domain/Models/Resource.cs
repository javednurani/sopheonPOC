using System.Collections.Generic;

namespace Sopheon.CloudNative.Environments.Domain.Models
{
   public class Resource : Entity
   {
      public int DomainResourceTypeId
      {
         get;
         set;
      }

      public string Uri
      {
         get;
         set;
      }

      public virtual DomainResourceType DomainResourceType
      {
         get;
         set;
      }

      public virtual DedicatedEnvironmentResource DedicatedEnvironmentResource
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
