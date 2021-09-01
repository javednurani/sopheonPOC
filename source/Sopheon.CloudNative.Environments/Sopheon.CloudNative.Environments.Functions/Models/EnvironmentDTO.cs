using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
