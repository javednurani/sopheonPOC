using System.Collections.Generic;

namespace Sopheon.CloudNative.Products.Domain
{
   public enum HealthReviewPeriods
   {
      NoPeriodicReviewScheduled = 0,
      Weekly = 1,
      Monthly = 2
   }

   public class HealthConfiguration
   {
      /// <summary>
      /// Foreign Key
      /// </summary>
      public int ProductId { get; set; }

      /// <summary>
      /// Navigation Property
      /// </summary>
      public Product Product { get; set; }

      /// <summary>
      /// Attributes the Product Should Have
      /// </summary>
      public List<Attribute> Attributes { get; set; }

      /// <summary>
      /// KPIs to manage for the product
      /// </summary>
      public List<KeyPerformanceIndicator> KeyPerformanceIndicators { get; set; }

      public HealthReviewPeriods HealthReviewReminderPeriod { get; set; }
   }
}
