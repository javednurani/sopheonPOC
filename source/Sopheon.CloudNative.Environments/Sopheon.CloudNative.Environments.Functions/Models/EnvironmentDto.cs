using System;

namespace Sopheon.CloudNative.Environments.Functions.Models
{
   public class EnvironmentDto
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

      public string Description
      {
         get;
         set;
      }

      public Guid Owner
      {
         get;
         set;
      }
   }
}
