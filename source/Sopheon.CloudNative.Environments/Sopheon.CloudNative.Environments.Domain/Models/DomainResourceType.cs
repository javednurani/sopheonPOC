
using System.Collections.Generic;

namespace Sopheon.CloudNative.Environments.Domain.Models
{
   public class DomainResourceType : Entity
   {
      public string Name
      {
         get;
         set;
      }

      public bool IsDedicated
      {
         get;
         set;
      }

      public virtual ICollection<BusinessServiceDependency> BusinessServiceDependencies
      {
         get;
         set;
      }

      public virtual ICollection<Resource> Resources
      {
         get;
         set;
      }
   }
}
