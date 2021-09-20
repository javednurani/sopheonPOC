using System;
using System.Collections.Generic;

namespace Sopheon.CloudNative.Environments.Domain.Models
{
   public class Environment
   {
      public int EnvironmentID
      {
         get;
         set;
      }

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
