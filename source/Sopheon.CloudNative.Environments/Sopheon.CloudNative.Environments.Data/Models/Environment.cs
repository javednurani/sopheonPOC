using System;
using System.ComponentModel.DataAnnotations;

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

      [MaxLength(64)]
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

      [MaxLength(1000)]
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
   }
}
