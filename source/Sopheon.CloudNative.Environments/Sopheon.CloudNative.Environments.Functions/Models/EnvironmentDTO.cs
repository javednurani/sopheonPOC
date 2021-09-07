using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Environments.Functions.Models
{
   // Cloud-1484, needed this at some point, but do not currently need in test/runtime
   // possibly related to: ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
   //[Serializable]
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
