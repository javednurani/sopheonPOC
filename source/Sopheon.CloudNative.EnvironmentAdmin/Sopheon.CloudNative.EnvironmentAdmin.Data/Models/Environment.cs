using System;
using System.ComponentModel.DataAnnotations;

namespace Sopheon.CloudNative.EnvironmentAdmin.Data.Models
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
