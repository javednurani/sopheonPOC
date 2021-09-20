
using System.Collections.Generic;

namespace Sopheon.CloudNative.Environments.Domain.Models
{
   public class ResourceType : Entity
   {
      public string Name
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
