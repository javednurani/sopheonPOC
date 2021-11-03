using System;
using System.Collections.Generic;

namespace Sopheon.CloudNative.Products.AspNetCore.HealthCheck
{
   public class IndividualHealthCheckResponse
   {
      public string Status { get; set; }
      public string Component { get; set; }
      public string Description { get; set; }
   }

   public class HealthCheckReponse
   {
      public string Status { get; set; }
      public IEnumerable<IndividualHealthCheckResponse> HealthChecks { get; set; }
      public TimeSpan HealthCheckDuration { get; set; }
   }
}
