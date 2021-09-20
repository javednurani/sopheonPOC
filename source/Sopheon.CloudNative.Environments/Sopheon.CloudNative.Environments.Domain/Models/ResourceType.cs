
using System.Collections.Generic;

namespace Sopheon.CloudNative.Environments.Domain.Models
{
   public class ResourceType
   {
      public int ResourceTypeID
      {
         get;
         set;
      }

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
   }
}
