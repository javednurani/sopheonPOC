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

      [MaxLength(100)]
      public string Name
      {
         get;
         set;
      }
   }
}
