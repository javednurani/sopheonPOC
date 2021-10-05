using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Environments.Functions.Helpers
{
   public interface IDatabaseBufferMonitorHelper
   {
      /// <summary>
      /// Query azure to see the avaiable, unavailable, and provisining databases.
      /// Check to see if unavailable + provisioning >= threshold value
      /// </summary>
      /// <returns>true, if the number of database available meets or exceeds the threshold</returns>
      Task<bool> CheckHasDatabaseThreshold();
   }
}
