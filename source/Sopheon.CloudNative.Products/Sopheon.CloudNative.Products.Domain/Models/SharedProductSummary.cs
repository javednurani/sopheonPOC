using System;
using System.Collections.Generic;

namespace Sopheon.CloudNative.Products.Domain
{
   public class SharedProductSummary
   {
      public int SharedProductSummaryId { get; set; }


      public int ProductId { get; set; }

      /// <summary>
      /// Navigation Property
      /// </summary>
      public Product Product { get; set; }

      public string Name { get; set; }

      /// <summary>
      /// Content such as: Timeline, Product Strategy, Planning List
      /// </summary>
      public List<string> IncludedContent { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public string PasswordHash { get; set; }
   }

   public class ShareEvent
   {
      public int SharedProductSummaryId { get; set; }

      /// <summary>
      /// Navigation Property
      /// </summary>
      public SharedProductSummary SharedProductSummary { get; set; }

      public List<string> Emails { get; set; }

      public string Note { get; set; }

      public DateTime TimeStamp { get; set; }
   }
}
