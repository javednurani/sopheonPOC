using Sopheon.CloudNative.Products.Domain.Attributes.Decimal;

namespace Sopheon.CloudNative.Products.Domain
{
   /// <summary>
   /// Can only be created from attributes with numeric AttributeDataTypes
   /// </summary>
   public class KeyPerformanceIndicator
   {
      public int KeyPerformanceIndicatorId { get; set; }

      public int AttributeId { get; set; }

      /// <summary>
      /// Navigation Property
      /// </summary>
      public DecimalAttribute Attribute { get; set; }

      // TODO: Other details that deal with the specific KPI concern, rather than just the attribute 'value holder' 
      // Min/Max?
      // Target?
   }
}
