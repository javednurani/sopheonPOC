using System;
using System.Collections.Generic;

namespace Sopheon.CloudNative.Environments.Domain.Models
{
   public class Environment : Entity
   {
      public Guid EnvironmentKey
      {
         get;
         set;
      }

      public string Name
      {
         get;
         set;
      }

      public Guid Owner
      {
         get;
         set;
      }

      public string Description
      {
         get;
         set;
      }

      public bool IsDeleted
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
