
using System.Collections.Generic;

namespace Sopheon.CloudNative.Environments.Domain.Models
{
   public class BusinessServiceDependency : Entity
   {
      public string DependencyName
      {
         get;
         set;
      }

      public int BusinessServiceId
      {
         get;
         set;
      }

      public virtual BusinessService BusinessService
      {
         get;
         set;
      }

      public int DomainResourceTypeId
      {
         get;
         set;
      }

      public virtual DomainResourceType DomainResourceType
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
