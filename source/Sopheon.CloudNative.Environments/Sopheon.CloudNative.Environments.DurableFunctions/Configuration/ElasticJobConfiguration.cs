using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Environments.DurableFunctions.Configuration
{
   public class ElasticJobConfiguration
   {
      public string ResourceGroupName { get; set; }
      public string ElasticJobAgentServerName { get; set; }
      public string ElasticJobAgentName { get; set; }
      public string TargetedSqlServerName { get; set;}
      public string ScheduledStartTime { get; set; }
      public string SqlCommandText { get; set; }
   }
}
