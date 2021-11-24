using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Products.Domain
{
   public class UtcDateTimeAttributeValue : IAttributeValue<DateTime?>
   {
      public int AttributeId { get; set; }

      public Attribute Attribute { get; set; }

      public DateTime? Value { get; set; }
   }
}
