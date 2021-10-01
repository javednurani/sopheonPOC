using System.Collections.Generic;

namespace Sopheon.CloudNative.Environments.Domain.Models
{
   public class BusinessService : Entity
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
   }
}
