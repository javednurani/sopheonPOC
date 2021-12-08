#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

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
      public Attribute Attribute { get; set; }

      // TODO: Other details that deal with the specific KPI concern, rather than just the attribute 'value holder' 
      // Min/Max?
      // Target?
   }
}
