using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Products.Domain
{

   /// <summary>
   /// A snapshot of an entity, and the period that it is valid for
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public class EntitySnapshot<T> where T : class
   {
      public T Snapshot { get; set; }
      public DateTime PeriodStart { get; set; }
      public DateTime PeriodEnd { get; set; }
   }
}
